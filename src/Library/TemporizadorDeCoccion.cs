namespace Full_GRASP_And_SOLID;

using System.Threading;

public class TemporizadorDeCoccion : TimerClient
{
    private Recipe receta;

    public TemporizadorDeCoccion(Recipe receta)
    {
        this.receta = receta;
    }

    public void TimeOut()
    {
        receta.Set_De_YaEstáCocinado();
    }
}