namespace backend.Domains.Movimentations
{
    public class MovimentationDtoIn
    {
        public Guid? Id { get; set; }
        public int Amount { get; set; }
        public Guid ProductId { get; set; }
    }
}
