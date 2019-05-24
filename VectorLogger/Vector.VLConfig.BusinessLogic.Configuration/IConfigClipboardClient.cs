using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public interface IConfigClipboardClient
	{
		List<Vector.VLConfig.Data.ConfigurationDataModel.Action> FocusedActions
		{
			get;
		}

		ConfigClipboardManager.AcceptType Accept(Vector.VLConfig.Data.ConfigurationDataModel.Action action);

		bool Insert(Vector.VLConfig.Data.ConfigurationDataModel.Action action);

		bool Insert(Event evt);
	}
}
