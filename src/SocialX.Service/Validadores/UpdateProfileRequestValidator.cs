using SocialX.Service.DTOs;
using FluentValidation;
using System.Text.RegularExpressions;

namespace SocialX.Service.Validadores;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.Apelido)
            .NotEmpty().WithMessage("Campo obrigatório")
            .MaximumLength(50).WithMessage("Apelido deve ter no máximo 50 caracteres");

        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("Campo obrigatório")
            .Must(BeValidBrazilianPhone).WithMessage("Formato de telefone inválido");

        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("Campo obrigatório")
            .Must(BeValidDate).WithMessage("Data de nascimento inválida")
            .Must(NotBeFutureDate).WithMessage("Data de nascimento não pode ser no futuro");

        RuleFor(x => x.FotoPerfil)
            .MaximumLength(500).WithMessage("URL da foto de perfil deve ter no máximo 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.FotoPerfil));

        RuleFor(x => x.Bio)
            .MaximumLength(500).WithMessage("Bio deve ter no máximo 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Bio));

        RuleFor(x => x.Cidade)
            .MaximumLength(100).WithMessage("Cidade deve ter no máximo 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Cidade));
    }

    private bool BeValidBrazilianPhone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
        {
            return false;
        }

        // Remove all non-digit characters
        string digitsOnly = Regex.Replace(telefone, @"\D", "");

        // Brazilian phone format: 10 or 11 digits (with or without country code)
        // Examples: 11987654321, 1198765432, (11) 98765-4321, (11) 9876-5432
        if (digitsOnly.Length == 10 || digitsOnly.Length == 11)
        {
            return true;
        }

        // With country code: 5511987654321 or 551198765432
        if (digitsOnly.Length == 12 || digitsOnly.Length == 13)
        {
            return digitsOnly.StartsWith("55");
        }

        return false;
    }

    private bool BeValidDate(DateTime date)
    {
        // Check if date is not default value and is a reasonable date
        return date != default && date.Year >= 1900 && date.Year <= DateTime.UtcNow.Year;
    }

    private bool NotBeFutureDate(DateTime date)
    {
        return date.Date <= DateTime.UtcNow.Date;
    }
}
