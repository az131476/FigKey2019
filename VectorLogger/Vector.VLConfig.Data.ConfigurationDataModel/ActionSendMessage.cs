using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "ActionSendMessage")]
	public class ActionSendMessage : Action, ISymbolicMessage, ITransmitMessageChannel
	{
		[DataMember(Name = "ActionSendMessageMessageData")]
		public List<DataItem> MessageData;

		[DataMember(Name = "ActionSendMessageBusType")]
		public ValidatedProperty<BusType> BusType
		{
			get;
			set;
		}

		[DataMember(Name = "ActionSendMessageChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		[DataMember(Name = "ActionSendMessageIsVirtual")]
		public ValidatedProperty<bool> IsVirtual
		{
			get;
			set;
		}

		[DataMember(Name = "ActionSendMessageIsSymbolic")]
		public ValidatedProperty<bool> IsSymbolic
		{
			get;
			set;
		}

		[DataMember(Name = "ActionSendMessageDatabaseName")]
		public ValidatedProperty<string> DatabaseName
		{
			get;
			set;
		}

		[DataMember(Name = "ActionSendMessageDatabasePath")]
		public ValidatedProperty<string> DatabasePath
		{
			get;
			set;
		}

		[DataMember(Name = "ActionSendMessageNetworkName")]
		public ValidatedProperty<string> NetworkName
		{
			get;
			set;
		}

		[DataMember(Name = "ActionSendMessageSymbolName")]
		public ValidatedProperty<string> SymbolName
		{
			get;
			set;
		}

		public ValidatedProperty<string> MessageName
		{
			get
			{
				return this.SymbolName;
			}
			set
			{
				this.SymbolName = value;
			}
		}

		[DataMember(Name = "ActionSendMessageID")]
		public ValidatedProperty<uint> ID
		{
			get;
			set;
		}

		[DataMember(Name = "ActionSendMessageIsExtendedId")]
		public ValidatedProperty<bool> IsExtendedId
		{
			get;
			set;
		}

		public int DLC
		{
			get
			{
				return this.MessageData.Count;
			}
			set
			{
				if (value > this.MessageData.Count)
				{
					while (this.MessageData.Count < value)
					{
						this.MessageData.Add(new DataItem());
					}
					return;
				}
				if (value < this.MessageData.Count)
				{
					while (this.MessageData.Count > value)
					{
						this.MessageData.RemoveAt(this.MessageData.Count - 1);
					}
				}
			}
		}

		public ActionSendMessage()
		{
			this.BusType = new ValidatedProperty<BusType>(Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN);
			this.ChannelNumber = new ValidatedProperty<uint>(1u);
			this.IsVirtual = new ValidatedProperty<bool>(true);
			this.IsSymbolic = new ValidatedProperty<bool>(false);
			this.DatabaseName = new ValidatedProperty<string>("");
			this.DatabasePath = new ValidatedProperty<string>("");
			this.NetworkName = new ValidatedProperty<string>("");
			this.SymbolName = new ValidatedProperty<string>("");
			this.ID = new ValidatedProperty<uint>(0u);
			this.IsExtendedId = new ValidatedProperty<bool>(false);
			this.MessageData = new List<DataItem>();
		}

		public ActionSendMessage(ActionSendMessage other) : this()
		{
			base.Assign(other);
			this.Assign(other);
		}

		public ActionSendMessage(Event ev, StopType stopType) : this()
		{
			this.Event = ev;
			this.StopType = stopType;
		}

		public override object Clone()
		{
			return new ActionSendMessage(this);
		}

		public override bool Equals(Action action)
		{
			if (action == null || base.GetType() != action.GetType())
			{
				return false;
			}
			ActionSendMessage actionSendMessage = (ActionSendMessage)action;
			if (this.BusType.Value != actionSendMessage.BusType.Value)
			{
				return false;
			}
			if (this.ChannelNumber.Value != actionSendMessage.ChannelNumber.Value)
			{
				return false;
			}
			if (this.IsVirtual.Value != actionSendMessage.IsVirtual.Value)
			{
				return false;
			}
			if (this.IsSymbolic.Value != actionSendMessage.IsSymbolic.Value)
			{
				return false;
			}
			if (this.DatabaseName.Value != actionSendMessage.DatabaseName.Value)
			{
				return false;
			}
			if (this.DatabasePath.Value != actionSendMessage.DatabasePath.Value)
			{
				return false;
			}
			if (this.NetworkName.Value != actionSendMessage.NetworkName.Value)
			{
				return false;
			}
			if (this.SymbolName.Value != actionSendMessage.SymbolName.Value)
			{
				return false;
			}
			if (this.ID.Value != actionSendMessage.ID.Value)
			{
				return false;
			}
			if (this.IsExtendedId.Value != actionSendMessage.IsExtendedId.Value)
			{
				return false;
			}
			if (this.MessageData.Count != actionSendMessage.MessageData.Count)
			{
				return false;
			}
			for (int i = 0; i < this.MessageData.Count; i++)
			{
				if (this.MessageData[i].Byte.Value != actionSendMessage.MessageData[i].Byte.Value)
				{
					return false;
				}
			}
			return base.Equals(action);
		}

		public override int GetHashCode()
		{
			int num = 0;
			foreach (DataItem current in this.MessageData)
			{
				num ^= (int)current.Byte.Value;
			}
			return this.BusType.Value.GetHashCode() ^ this.ChannelNumber.Value.GetHashCode() ^ this.IsVirtual.Value.GetHashCode() ^ this.IsSymbolic.Value.GetHashCode() ^ this.DatabaseName.Value.GetHashCode() ^ this.DatabasePath.Value.GetHashCode() ^ this.NetworkName.Value.GetHashCode() ^ this.SymbolName.Value.GetHashCode() ^ this.ID.Value.GetHashCode() ^ this.IsExtendedId.Value.GetHashCode() ^ num ^ base.GetHashCode();
		}

		public void Assign(ActionSendMessage other)
		{
			this.BusType.Value = other.BusType.Value;
			this.ChannelNumber.Value = other.ChannelNumber.Value;
			this.IsVirtual.Value = other.IsVirtual.Value;
			this.IsSymbolic.Value = other.IsSymbolic.Value;
			this.DatabaseName.Value = other.DatabaseName.Value;
			this.DatabasePath.Value = other.DatabasePath.Value;
			this.NetworkName.Value = other.NetworkName.Value;
			this.SymbolName.Value = other.SymbolName.Value;
			this.ID.Value = other.ID.Value;
			this.IsExtendedId.Value = other.IsExtendedId.Value;
			this.DLC = other.DLC;
			for (int i = 0; i < this.DLC; i++)
			{
				this.MessageData[i].Byte.Value = other.MessageData[i].Byte.Value;
			}
			base.Assign(other);
		}
	}
}
