using System;

namespace Indigo.CrossCutting.Utilities.Metrics.Monitor
{
	public class MonitorBase : 
		IMonitor
	{
		private readonly string _name;
		private readonly Type _ownerType;

		public MonitorBase(Type ownerType, string name)
		{
			_ownerType = ownerType;
			_name = name;
		}

		public Type OwnerType
		{
			get { return _ownerType; }
		}

		public string Name
		{
			get { return _name; }
		}
	}
}