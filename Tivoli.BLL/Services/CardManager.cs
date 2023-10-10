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
        IEnumerable<Card> cards = _repo.GetAll(c => c.CustomerId == userId);
        return cards.Adapt<IEnumerable<CardDto>>();
    }
}