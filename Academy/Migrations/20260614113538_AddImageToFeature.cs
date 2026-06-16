using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    public partial class AddImageToFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Image sütununu yalnız mövcud deyilsə əlavə et
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Feature' AND COLUMN_NAME = 'Image'
                )
                BEGIN
                    ALTER TABLE [Feature] ADD [Image] nvarchar(max) NULL;
                END
            ");

            // Orders cədvəlini yalnız mövcud deyilsə yarat
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Orders' AND xtype='U')
                BEGIN
                    CREATE TABLE [Orders] (
                        [Id] int NOT NULL IDENTITY,
                        [AppUserId] nvarchar(450) NOT NULL,
                        [OrderNumber] nvarchar(max) NOT NULL,
                        [FullName] nvarchar(max) NOT NULL,
                        [Email] nvarchar(max) NOT NULL,
                        [Phone] nvarchar(max) NOT NULL,
                        [TotalAmount] decimal(18,2) NOT NULL,
                        [PaymentMethod] nvarchar(max) NOT NULL,
                        [StripePaymentIntentId] nvarchar(max) NULL,
                        [Status] int NOT NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_Orders_AspNetUsers_AppUserId] FOREIGN KEY ([AppUserId]) 
                            REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
                    );
                END
            ");

            // OrderItems cədvəlini yalnız mövcud deyilsə yarat
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='OrderItems' AND xtype='U')
                BEGIN
                    CREATE TABLE [OrderItems] (
                        [Id] int NOT NULL IDENTITY,
                        [OrderId] int NOT NULL,
                        [CourseId] int NOT NULL,
                        [CourseTitle] nvarchar(max) NOT NULL,
                        [Price] decimal(18,2) NOT NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) 
                            REFERENCES [Orders] ([Id]) ON DELETE CASCADE,
                        CONSTRAINT [FK_OrderItems_Courses_CourseId] FOREIGN KEY ([CourseId]) 
                            REFERENCES [Courses] ([Id]) ON DELETE CASCADE
                    );
                END
            ");

            // Index-ləri yalnız mövcud deyilsə yarat
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Orders_AppUserId')
                    CREATE INDEX [IX_Orders_AppUserId] ON [Orders] ([AppUserId]);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_OrderItems_OrderId')
                    CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_OrderItems_CourseId')
                    CREATE INDEX [IX_OrderItems_CourseId] ON [OrderItems] ([CourseId]);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sysobjects WHERE name='OrderItems' AND xtype='U') DROP TABLE [OrderItems];");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sysobjects WHERE name='Orders' AND xtype='U') DROP TABLE [Orders];");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Feature' AND COLUMN_NAME='Image') ALTER TABLE [Feature] DROP COLUMN [Image];");
        }
    }
}
