using System.Threading.Tasks;

namespace grid.entities.units
{
    public interface IControllableEntity
    {
        Task Aim();
        Task Shoot();
        Task ResetAim();
    }

    public interface ICommand
    {
        Task Execute();
    }

    public abstract class Command: ICommand
    {
        protected IControllableEntity entity;

        protected Command(IControllableEntity entity)
        {
            this.entity = entity;
        }
        
        public abstract Task Execute();

        public static T Create<T>(IControllableEntity entity) where T : Command
        {
            return (T)System.Activator.CreateInstance(typeof(T), entity);
        }
        
    }

    public class ShootCommand : Command
    {
        public ShootCommand(IControllableEntity entity) : base(entity)
        {
        }
        
        public override async Task Execute()
        {
            await entity.Aim();
            await entity.Shoot();
            await entity.ResetAim();
        }
    }
    
    
}