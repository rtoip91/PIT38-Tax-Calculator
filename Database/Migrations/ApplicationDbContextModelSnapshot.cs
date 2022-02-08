﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("Database.Entities.CfdEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ClosingRate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrencySymbol")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ExchangeRate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("GainExchangedValue")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("GainValue")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("LossExchangedValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OpeningRate")
                        .HasColumnType("TEXT");

                    b.Property<long>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SellDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Units")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CfdCalculations");
                });

            modelBuilder.Entity("Database.Entities.ClosedPositionEntity", b =>
                {
                    b.Property<int?>("PositionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ClosingDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("ClosingRate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Comments")
                        .HasColumnType("TEXT");

                    b.Property<string>("CopiedInvestor")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("FeesAndDividends")
                        .HasColumnType("TEXT");

                    b.Property<string>("IsReal")
                        .HasColumnType("TEXT");

                    b.Property<int>("Leverage")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("OpeningDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("OpeningRate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Operation")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Profit")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Spread")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("StopLossRate")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("TakeProfitRate")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Units")
                        .HasColumnType("TEXT");

                    b.HasKey("PositionId");

                    b.ToTable("ClosedPositions");
                });

            modelBuilder.Entity("Database.Entities.CryptoEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ClosingExchangeRate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ClosingValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrencySymbol")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("GainExchangedValue")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("LossExchangedValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OpeningExchangeRate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OpeningValue")
                        .HasColumnType("TEXT");

                    b.Property<long>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Profit")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SellDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CryptoCalculations");
                });

            modelBuilder.Entity("Database.Entities.ExchangeRateEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<string>("Currency")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Rate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ExchangeRates");
                });

            modelBuilder.Entity("Database.Entities.StockEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ClosingExchangeRate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ClosingExchangedValue")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ClosingUnitValue")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ClosingValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrencySymbol")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ExchangedProfit")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OpeningExchangeRate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OpeningExchangedValue")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OpeningUnitValue")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OpeningValue")
                        .HasColumnType("TEXT");

                    b.Property<long>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Profit")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SellDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Units")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("StockCalculations");
                });

            modelBuilder.Entity("Database.Entities.TransactionReportEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("AccountBalance")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Details")
                        .HasColumnType("TEXT");

                    b.Property<int>("NWA")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("RealizedEquity")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("RealizedEquityChange")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.ToTable("TransactionReports");
                });

            modelBuilder.Entity("Database.Entities.TransactionReportEntity", b =>
                {
                    b.HasOne("Database.Entities.ClosedPositionEntity", "ClosedPosition")
                        .WithMany("TransactionReports")
                        .HasForeignKey("PositionId");

                    b.Navigation("ClosedPosition");
                });

            modelBuilder.Entity("Database.Entities.ClosedPositionEntity", b =>
                {
                    b.Navigation("TransactionReports");
                });
#pragma warning restore 612, 618
        }
    }
}
