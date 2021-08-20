using Volo.Abp.Settings;

namespace RolesDemo.Settings
{
    public class RolesDemoSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(RolesDemoSettings.MySetting1));
        }
    }
}
