using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsaTec.Model;

namespace MsaTec.DAL.FluentApiMappingConfiguration;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {

        // Define mapeamento da entidade Cliente para a tabela correspondente no PostgreSQL.
        builder.ToTable("Clientes");

        // Configuração da chave primária.
        builder.HasKey(e => e.Id);
        
        // Configuração do campo Nome.
        builder.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        // Configuração do campo Email.
        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnType("varchar(255)");

        // Configuração do campo DataNascimento.
        builder.Property(e => e.DataNascimento)
            .HasColumnType("date");

        // Configuração da relação com a entidade Telefone (um Cliente pode ter vários Telefones).
        builder.HasMany(e => e.Telefones)
            .WithOne(c => c.Cliente); // Permite a exclusão em cascata dos telefones quando um cliente for excluído.

        // Outras configurações, como índices, podem ser adicionadas conforme necessário.
        builder.Property(v => v.CreatedWhen).HasColumnName("CriadoEm").IsRequired().HasDefaultValueSql("CURRENT_DATE");
        builder.Property(v => v.UpdatedWhen).HasColumnName("AtualzadoEm");
        // Configure relationships, indexes, and other constraints as needed.

        builder.HasIndex(c => c.Email).IsUnique();
    }
}
