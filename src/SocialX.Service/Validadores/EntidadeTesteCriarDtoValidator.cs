using SocialX.Service.DTOs;
using FluentValidation;

namespace SocialX.Service.Validadores;

public class EntidadeTesteCriarDtoValidator : AbstractValidator<EntidadeTesteCriarDto>
{
    public EntidadeTesteCriarDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(150).WithMessage("Nome deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("Valor deve ser maior que zero.");
    }
}
