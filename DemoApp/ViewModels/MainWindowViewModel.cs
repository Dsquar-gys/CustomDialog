﻿using CustomDialogLibrary.ViewModels;

namespace DemoApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public GeneralViewModel GeneralViewModel => new();
}