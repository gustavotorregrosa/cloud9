namespace backend.Domains.Movimentations
{
    public class MovimentationDtoOut : MovimentationDtoIn
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
