using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnDPartyManagerMobile.VM;

namespace DnDPartyManagerMobile.V;

public partial class EnemyPage : ContentPage
{
    public EnemyPage()
    {
        InitializeComponent();
        BindingContext = new EnemiesViewModel();
    }
}