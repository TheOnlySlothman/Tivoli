using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Tivoli.BLL.DTO;
using Tivoli.Dal.Entities;
using Tivoli.Dal.Repo;

namespace Tivoli.BLL.Services;

public class CardManager
{
    private readonly UnitOfWork _unitOfWork;
    private readonly CardRepo _repo;

    public CardManager(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repo = unitOfWork.Cards;
    }

    public void CreateCard(Customer user)
    {
        string data = string.Create(1028, 'a', (span, value) => span.Fill(value));
        Card card = new(data, user);
        _repo.Add(card);
        _unitOfWork.SaveChanges();
    }

    public IEnumerable<CardDto> GetUserCards(Guid userId)
    {
        // IEnumerable<Card> cards = _repo.GetAll(c => c.CustomerId == userId);
        IEnumerable<Card> cards = _repo.GetCardsByCustomerId(userId);
        return cards.Adapt<IEnumerable<CardDto>>();
    }

    public ActionResult AddBalance(Guid userId, Guid cardId, decimal amount)
    {
        if (amount <= 0) return new BadRequestObjectResult("Amount must be positive");
        if (!_repo.Exists(cardId)) return new BadRequestObjectResult("Card does not exist");

        Card card = _repo.Get(cardId);
        if (card.CustomerId != userId) return new BadRequestObjectResult("Card does not belong to user");

        card.Balance += amount;
        _unitOfWork.SaveChanges();
        return new OkResult();
    }

    public ActionResult ValidateAndPayWithCard(string cardData, decimal price)
    {
        if (!_repo.Exists(cardData))
            return new BadRequestObjectResult("Card does not exist");

        Card card = _repo.GetByCardData(cardData);

        if (card.Balance < price)
            return new BadRequestObjectResult("Not enough money on card");

        card.Balance -= price;
        _unitOfWork.SaveChanges();
        return new OkResult();
    }
}