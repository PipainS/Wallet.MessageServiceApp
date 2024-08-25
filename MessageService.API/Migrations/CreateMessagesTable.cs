using FluentMigrator;

namespace MessageService.API.Migrations
{
    [Migration(202308160001)]
    public class CreateMessagesTable : Migration
    {
        public override void Up()
        {
            Create.Table("messages")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("text").AsString(128).NotNullable()
                .WithColumn("user_name").AsString(128).Nullable()
                .WithColumn("timestamp").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("user_id").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("messages");
        }
    }
}