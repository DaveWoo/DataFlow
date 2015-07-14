using System;
using System.Runtime.Serialization;
using System.Xml;

namespace DataFlow
{
	public class SharedTypeResolver : DataContractResolver
	{
		public override bool TryResolveType(Type dataContractType, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
		{
			try
			{
				if (!knownTypeResolver.TryResolveType(dataContractType, declaredType, null, out typeName, out typeNamespace))
				{
					XmlDictionary dictionary = new XmlDictionary();
					typeName = dictionary.Add(dataContractType.FullName);
					typeNamespace = dictionary.Add(dataContractType.Assembly.FullName);
				}
			}
			catch (Exception)
			{
				throw;
			}
			return true;
		}

		public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
		{
			Type type = null;
			try
			{
				type = knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null);
			}
			catch (Exception)
			{
				throw;
			}

			return type ?? declaredType;
		}
	}
}