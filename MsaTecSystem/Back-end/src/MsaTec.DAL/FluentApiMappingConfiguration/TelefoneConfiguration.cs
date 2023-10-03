using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MsaTec.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsaTec.DAL.FluentApiMappingConfiguration;

public class TelefoneConfiguration : IEntityTypeConfiguration<Telefone>
{
    public void Configure(EntityTypeBuilder<Telefone> builder)
    {
        // Define mapeamento da entidade Telefone para a tabela correspondente no PostgreSQL.
        builder.ToTable("Telefones");

        // Configuração da chave primária.
        builder.HasKey(e => e.Id);
        
        // Configuração do campo Numero.
        builder.Property(e => e.Numero)
            .IsRequired()
            .HasColumnType("varchar(20)"); // Ajuste o tamanho conforme necessário.

        // Configuração do campo Tipo.
        builder.Property(e => e.Tipo)
            .IsRequired()
            .HasMaxLength(20) // Ajuste o tamanho conforme necessário.
            .HasColumnType("varchar(20)");

        // Configuração da relação com a entidade Cliente (um Telefone pertence a um Cliente).
        builder.HasOne(e => e.Cliente)
            .WithMany(c => c.Telefones)
            .HasForeignKey(e => e.ClienteId)
            .OnDelete(DeleteBehavior.Cascade); // Ajuste o comportamento de exclusão conforme necessário.

        builder.Property(v => v.CreatedWhen).HasColumnName("CriadoEm").IsRequired().HasDefaultValueSql("CURRENT_DATE");
        builder.Property(v => v.UpdatedWhen).HasColumnName("AtualzadoEm");
        // Add any other configurations you need.

        //builder.ToTable(t => t.HasCheckConstraint("chkTelefoneNumero", "numero ~ '^[0-9]+$' AND length(numero) >= 7"));
        //builder.ToTable(t => t.HasCheckConstraint("chkTelefoneTipo", "[Tipo] IN (1, 2, 3, 4)"));
    }
}
