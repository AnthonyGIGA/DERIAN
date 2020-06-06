using System.ComponentModel;
using System.Runtime.CompilerServices;

public class viewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private string valor_;
    public string valor { get { return valor_; } set { if (valor_ != value) { valor_ = ProcessAge(value); OnPropertyChanged(); } } }

    private string ProcessAge(string Valor)
    {
        if (string.IsNullOrEmpty(Valor))
            return Valor; 

        return Valor;
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}