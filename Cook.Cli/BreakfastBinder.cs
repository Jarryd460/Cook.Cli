using System.CommandLine;
using System.CommandLine.Binding;

namespace Cook.Cli
{
    //internal class BreakfastBinder : BinderBase<BreakfastCommandOptions>
    //{
    //    private readonly Option _meal;

    //    public BreakfastBinder(Option meal)
    //    {
    //        _meal = meal;
    //    }

    //    protected override BreakfastCommandOptions GetBoundValue(BindingContext bindingContext)
    //    {
    //        var meal = bindingContext.ParseResult.GetValueForOption<string>(_meal);

    //        return new BreakfastCommandOptions()
    //        {
    //            Meal = meal ?? "Omellete"
    //        };
    //    }
    //}
}
