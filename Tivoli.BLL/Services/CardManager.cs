using Mapster;
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

    public bool AddBalance(Guid userId, Guid cardId, decimal amount)
    {
        if (amount <= 0) return false;
        if (!_repo.Exists(cardId)) return false;

        Card card = _repo.Get(cardId);
        if (card.CustomerId != userId) return false;

        card.Balance += amount;
        _unitOfWork.SaveChanges();
        return true;
    }
}