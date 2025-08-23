using System;

using CommunityToolkit.Mvvm.ComponentModel;

namespace ASVLM.Common.Libraries;

/// <summary>
///		Base class for model classes interacting with UI.
/// </summary>
public abstract class Model : ObservableObject
{
	public Model()
	{ }
	
	public void raisePropertyChanged(string? property_name)
	{
		OnPropertyChanged(property_name);
	}
}