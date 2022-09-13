using Ipe.Domain.Errors;
using Ipe.Domain.Models;
using Ipe.Helpers;
using Ipe.UseCases.Interfaces;
using Ipe.UseCases.Interfaces.Repositories;
using Ipe.UseCases.Interfaces.Services;

namespace Ipe.UseCases.PlantUseCase.CreatePlant
{
    public class PlantUseCase : IUseCase<PlantUseCaseInput, PlantUseCaseOutput>
    {
        private readonly ITreeRepository _treeRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPlantRepository _plantRepository;
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public PlantUseCase(
            ITreeRepository treeRepository,
            IOrderRepository orderRepository,
            IPlantRepository plantRepository,
            IPaymentService paymentService,
            IEmailService emailService,
            IUserRepository userRepository
        )
        {
            _treeRepository = treeRepository;
            _orderRepository = orderRepository;
            _plantRepository = plantRepository;
            _paymentService = paymentService;
            _emailService = emailService;
            _userRepository = userRepository;
        }

        public async Task<PlantUseCaseOutput> Run(PlantUseCaseInput Input)
        {
            List<Tree> Trees = await GetTreesById(Input.Trees);
            bool InvalidTrees = HasInvalidTree(Trees);

            if (InvalidTrees)
                throw new InvalidTreeIdException();

            Order Order = await CreateOrder(Input, Trees);
            var User = await _userRepository.GetById(Input.UserId);
            var PaymentResult = await ExecutePayment(Input, Order, Trees);

            if(PaymentResult.Success)
            {
                await UpdateOrderSucess(Order, PaymentResult);
                await HandleFirstPlant(User);
                await CreatePlant(Order, Trees);
                await _emailService.SendPlantSuccessEmail(User.Email, User.Name, Order, Trees);
            }
            else
            {
                await UpdateOrderFail(Order);
            }

            var OrderId = Order.Id;
            var Planted = false;

            if(PaymentResult is not null)
            {
                Planted = PaymentResult.Success;
            }

            return new PlantUseCaseOutput
            {
                OrderId = Planted ? OrderId : null,
                Planted = Planted,
            };
        }

        private async Task HandleFirstPlant(User User)
        {
            var UserHasPlant = await _plantRepository.FindSomePlantByUserId(User.Id);

            if(UserHasPlant is null)
            {
                await _emailService.SendFirstPlantEmail(User.Email, User.Name);
            }
        }

        private async Task UpdateOrderFail(Order order)
        {
            order.Status = PaymentStatus.DECLINED.ToString();
            await _orderRepository.Update(order);
        }

        private async Task UpdateOrderSucess(Order order, NewPaymentOutput paymentResult)
        {
            order.Status = PaymentStatus.PAID.ToString();
            order.PaymentId = paymentResult.PaymentId;
            await _orderRepository.Update(order);
        }

        private async Task CreatePlant(Order order, List<Tree> Trees)
        {

            var TreesToPlant = new List<Plant>();

            foreach(var Tree in order.Trees)
            {

                var CurrentTree = Trees.Find(x => x.Id == Tree.Id);

                if (CurrentTree is null)
                {
                    return;
                }

                for(var i = 0; i< Tree.Quantity; i++)
                {
                    TreesToPlant.Add(new Plant {
                         OrderId = order.Id,
                         UserId = order.UserId,
                         TreeId = Tree.Id,
                         Name = null,
                         Message = null,
                         Species = CurrentTree.Name,
                         Biome = CurrentTree.Biome,
                         Value = CurrentTree.Value,
                         Image = CurrentTree.Image,
                         Description = CurrentTree.Description,
                         Hastags = new List<string>(),
                    });
                }
            }

            await _plantRepository.CreateMany(TreesToPlant);
        }

        private async Task<NewPaymentOutput> ExecutePayment(
            PlantUseCaseInput Input,
            Order Order,
            List<Tree> Trees
        )
        {
            double PaymentValue = GetPaymentTotalValue(Trees, Input);

            var NewPayment = new NewPaymentInput
            {
                OrderId = $"IPE#{Order.Id}#{Input.UserId}",
                CardToken = Input.CardToken,
                Description = "Plant Payment",
                Value = PaymentValue
            };

            return await _paymentService.NewPayment(NewPayment);
        }

        private static double GetPaymentTotalValue(List<Tree> trees, PlantUseCaseInput Input)
        {
            double TotalValue = 0;

            foreach(var Tree in Input.Trees)
            {
                var TreeModel = trees.Find(x => x.Id == Tree.Id);

                if(TreeModel is not null)
                {
                    TotalValue += Tree.Quantity * TreeModel.Value;
                }

            }

            return TotalValue;
        }

        private async Task<Order> CreateOrder(PlantUseCaseInput Input, List<Tree> Trees)
        {
            var OrderValue = GetPaymentTotalValue(Trees, Input);

            var order = new Order
            {
                Status = PaymentStatus.CREATED.ToString(),
                UserId = Input.UserId,
                Trees = Input.Trees
                    .Select(tree => new OrderTree
                    {
                        Id = tree.Id,
                        Quantity = tree.Quantity
                    })
                    .ToList(),
                CreatedAt = DateHelper.BrazilDateTimeNow(),
                UpdatedAt = DateHelper.BrazilDateTimeNow(),
                PaymentId = null,
                Value = OrderValue
            };

            await _orderRepository.Create(order);

            return order;
        }

        private async Task<List<Tree>> GetTreesById(List<TreeUseCaseInput> trees)
        {
            var TreesId = trees.Select(t => t.Id).ToList();
            var AllTrees = await _treeRepository.GetTreesById(TreeId: TreesId);
            return AllTrees.ToList();
        }

        private static bool HasInvalidTree(List<Tree> Trees)
        {
            if (Trees is null || !Trees.Any())
                return true;

            return Trees.Any(tree => tree.Deleted);
        }     
    }
}
