using FluentMigrator;

[Migration(202308160001)]
public class CreateMessagesTable : Migration
{
    public override void Up()
    {
        Create.Table("messages")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("text").AsString(128).NotNullable()
            .WithColumn("timestamp").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("client_order").AsInt32().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("messages");
    }
}
