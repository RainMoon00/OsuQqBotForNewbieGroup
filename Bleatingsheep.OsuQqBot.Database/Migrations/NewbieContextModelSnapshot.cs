﻿// <auto-generated />
using System;
using Bleatingsheep.OsuQqBot.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bleatingsheep.OsuQqBot.Database.Migrations
{
    [DbContext(typeof(NewbieContext))]
    partial class NewbieContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Bleatingsheep.Osu.PerformancePlus.BeatmapPlus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<float>("Accuracy")
                        .HasColumnType("float");

                    b.Property<float>("AimFlow")
                        .HasColumnType("float");

                    b.Property<float>("AimJump")
                        .HasColumnType("float");

                    b.Property<float>("AimTotal")
                        .HasColumnType("float");

                    b.Property<float>("Precision")
                        .HasColumnType("float");

                    b.Property<float>("Speed")
                        .HasColumnType("float");

                    b.Property<float>("Stamina")
                        .HasColumnType("float");

                    b.Property<float>("Stars")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("BeatmapPlusCache");
                });

            modelBuilder.Entity("Bleatingsheep.OsuMixedApi.Beatmap", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("Mode")
                        .HasColumnType("int");

                    b.Property<double>("AR")
                        .HasColumnType("double");

                    b.Property<int>("Approved")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("ApprovedDateOffset")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Artist")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<double>("Bpm")
                        .HasColumnType("double");

                    b.Property<double>("CS")
                        .HasColumnType("double");

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DifficultyName")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("FavoriteCount")
                        .HasColumnType("int");

                    b.Property<string>("FileMD5")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("Genre")
                        .HasColumnType("int");

                    b.Property<double>("HP")
                        .HasColumnType("double");

                    b.Property<int>("HitLength")
                        .HasColumnType("int");

                    b.Property<int>("Language")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("LastUpdateOffset")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("MaxCombo")
                        .HasColumnType("int");

                    b.Property<double>("OD")
                        .HasColumnType("double");

                    b.Property<int>("SetId")
                        .HasColumnType("int");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<double>("Stars")
                        .HasColumnType("double");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("TotalLength")
                        .HasColumnType("int");

                    b.HasKey("Id", "Mode");

                    b.HasIndex("FileMD5")
                        .HasName("index_md5");

                    b.ToTable("CachedBeatmaps");
                });

            modelBuilder.Entity("Bleatingsheep.OsuQqBot.Database.Models.BindingInfo", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("OsuId")
                        .IsConcurrencyToken()
                        .HasColumnType("int");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId");

                    b.ToTable("Bindings");
                });

            modelBuilder.Entity("Bleatingsheep.OsuQqBot.Database.Models.OperationHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Operation")
                        .HasColumnType("int");

                    b.Property<string>("Operator")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<long?>("OperatorId")
                        .HasColumnType("bigint");

                    b.Property<string>("Remark")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("User")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Histories");
                });

            modelBuilder.Entity("Bleatingsheep.OsuQqBot.Database.Models.PlusHistory", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Accuracy")
                        .HasColumnType("int");

                    b.Property<int>("AimFlow")
                        .HasColumnType("int");

                    b.Property<int>("AimJump")
                        .HasColumnType("int");

                    b.Property<int>("AimTotal")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Performance")
                        .HasColumnType("int");

                    b.Property<int>("Precision")
                        .HasColumnType("int");

                    b.Property<int>("Speed")
                        .HasColumnType("int");

                    b.Property<int>("Stamina")
                        .HasColumnType("int");

                    b.HasKey("Id", "Date");

                    b.ToTable("PlusHistories");
                });

            modelBuilder.Entity("Bleatingsheep.OsuQqBot.Database.Models.RelationshipInfo", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Relationship")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("Target")
                        .IsConcurrencyToken()
                        .HasColumnType("int");

                    b.HasKey("UserId", "Relationship");

                    b.ToTable("Relationships");
                });

            modelBuilder.Entity("Bleatingsheep.OsuQqBot.Database.Models.UserPlayRecord", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Mode")
                        .HasColumnType("int");

                    b.Property<int>("PlayNumber")
                        .HasColumnType("int");

                    b.HasKey("UserId", "Mode", "PlayNumber");

                    b.ToTable("UserPlayRecords");
                });

            modelBuilder.Entity("Bleatingsheep.OsuQqBot.Database.Models.UserSnapshot", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Mode")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserSnapshots");
                });

            modelBuilder.Entity("Bleatingsheep.OsuQqBot.Database.Models.WebLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("IPAddress")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Kind")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Location")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTimeOffset>("Time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Token")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("User")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("WebLogs");
                });

            modelBuilder.Entity("Bleatingsheep.OsuQqBot.Database.Models.UserPlayRecord", b =>
                {
                    b.OwnsOne("Bleatingsheep.Osu.ApiClient.UserRecent", "Record", b1 =>
                        {
                            b1.Property<int>("UserPlayRecordUserId")
                                .HasColumnType("int");

                            b1.Property<int>("UserPlayRecordMode")
                                .HasColumnType("int");

                            b1.Property<int>("UserPlayRecordPlayNumber")
                                .HasColumnType("int");

                            b1.Property<int>("BeatmapId")
                                .HasColumnType("int");

                            b1.Property<int>("Count100")
                                .HasColumnType("int");

                            b1.Property<int>("Count300")
                                .HasColumnType("int");

                            b1.Property<int>("Count50")
                                .HasColumnType("int");

                            b1.Property<int>("CountGeki")
                                .HasColumnType("int");

                            b1.Property<int>("CountKatu")
                                .HasColumnType("int");

                            b1.Property<int>("CountMiss")
                                .HasColumnType("int");

                            b1.Property<DateTime>("Date")
                                .HasColumnType("datetime(6)");

                            b1.Property<int>("EnabledMods")
                                .HasColumnType("int");

                            b1.Property<int>("MaxCombo")
                                .HasColumnType("int");

                            b1.Property<bool>("Perfect")
                                .HasColumnType("tinyint(1)");

                            b1.Property<string>("Rank")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<long>("Score")
                                .HasColumnType("bigint");

                            b1.Property<int>("UserId")
                                .HasColumnType("int");

                            b1.HasKey("UserPlayRecordUserId", "UserPlayRecordMode", "UserPlayRecordPlayNumber");

                            b1.ToTable("UserPlayRecords");

                            b1.WithOwner()
                                .HasForeignKey("UserPlayRecordUserId", "UserPlayRecordMode", "UserPlayRecordPlayNumber");
                        });
                });

            modelBuilder.Entity("Bleatingsheep.OsuQqBot.Database.Models.UserSnapshot", b =>
                {
                    b.OwnsOne("Bleatingsheep.Osu.ApiClient.UserInfo", "UserInfo", b1 =>
                        {
                            b1.Property<long>("UserSnapshotId")
                                .HasColumnType("bigint");

                            b1.Property<double>("AccuracyPercent")
                                .HasColumnType("double");

                            b1.Property<int>("Count100")
                                .HasColumnType("int");

                            b1.Property<int>("Count300")
                                .HasColumnType("int");

                            b1.Property<int>("Count50")
                                .HasColumnType("int");

                            b1.Property<int>("CountRankA")
                                .HasColumnType("int");

                            b1.Property<int>("CountRankS")
                                .HasColumnType("int");

                            b1.Property<int>("CountRankSH")
                                .HasColumnType("int");

                            b1.Property<int>("CountRankSS")
                                .HasColumnType("int");

                            b1.Property<int>("CountRankSSH")
                                .HasColumnType("int");

                            b1.Property<string>("CountryCode")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<int>("CountryRank")
                                .HasColumnType("int");

                            b1.Property<long>("Id")
                                .HasColumnType("bigint");

                            b1.Property<DateTime>("JoinDate")
                                .HasColumnType("datetime(6)");

                            b1.Property<double>("Level")
                                .HasColumnType("double");

                            b1.Property<string>("Name")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<double>("Performance")
                                .HasColumnType("double");

                            b1.Property<int>("PlayCount")
                                .HasColumnType("int");

                            b1.Property<int>("Rank")
                                .HasColumnType("int");

                            b1.Property<long>("RankedScore")
                                .HasColumnType("bigint");

                            b1.Property<long>("TotalScore")
                                .HasColumnType("bigint");

                            b1.Property<int>("TotalSecondsPlayed")
                                .HasColumnType("int");

                            b1.HasKey("UserSnapshotId");

                            b1.ToTable("UserSnapshots");

                            b1.WithOwner()
                                .HasForeignKey("UserSnapshotId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
