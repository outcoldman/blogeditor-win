// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.BlogEditor.Model.MetaWeblog
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    using OutcoldSolutions.BlogEditor.Diagnostics;

    public class XmlRpcService : IXmlRpcService
    {
        private static readonly Type TypeString = typeof(string);

        private readonly ILogger logger;

        public XmlRpcService(ILogManager logManager)
        {
            this.logger = logManager.CreateLogger(typeof(XmlRpcService).Name);
        }

        public string CreateRequestBody(string methodName, params object[] parameters)
        {
            if (methodName == null)
            {
                throw new ArgumentNullException("methodName");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (this.logger.IsDebugEnabled)
            {
                this.logger.Debug("Creating request body for: '{0}'.", methodName);
            }

            var xMethodCall = new XElement("methodCall", new XElement("methodName", methodName));
            var xParams = new XElement("params");

            if (this.logger.IsDebugEnabled)
            {
                this.logger.Debug("Number of parameters: {0}.", parameters.Length);
            }

            foreach (var parameter in parameters)
            {
                xParams.Add(new XElement("param", this.ParameterToXml(parameter)));
            }

            xMethodCall.Add(xParams);

            var requestBody = xMethodCall.ToString(SaveOptions.DisableFormatting);

            if (this.logger.IsInfoEnabled)
            {
                this.logger.Info("Request body: '{0}'.", requestBody);
            }

            if (this.logger.IsDebugEnabled)
            {
                this.logger.Debug("Request created body for: '{0}'.", methodName);
            }

            return requestBody;
        }

        public XmlRpcResponse ParseResponse(string responseBody)
        {
            if (responseBody == null)
            {
                throw new ArgumentNullException("responseBody");
            }

            this.logger.Debug("Parsing response.");

            if (this.logger.IsInfoEnabled)
            {
                this.logger.Info("Response body: '{0}'.", responseBody);
            }

            var response = new XmlRpcResponse();

            var xResponse = XElement.Parse(responseBody);
            if (string.Equals(xResponse.Name.LocalName, "methodResponse", StringComparison.OrdinalIgnoreCase))
            {
                var xParams = xResponse.Element("params");
                if (xParams != null)
                {
                    var xParamsElements = xParams.Elements("param");
                    foreach (var xParam in xParamsElements)
                    {
                        var entity = this.ParseValue(xParam.Element("value"));
                        if (entity != null)
                        {
                            response.Parameters.Add(entity);
                        }
                    }
                }
                else
                {
                    this.logger.Warning("Response does not contain any params.");
                }
            }
            else
            {
                this.logger.Error("Could not find 'methodResponse' element in response.");
            }

            return response;
        }

        private XmlRpcEntity ParseValue(XElement xValue)
        {
            if (xValue != null)
            {
                // Value should has only one element
                var xValueElement = xValue.Elements().FirstOrDefault();
                
                if (string.Equals(xValueElement.Name.LocalName, "array", StringComparison.OrdinalIgnoreCase))
                {
                    return this.ParseArray(xValueElement);
                }
                
                if (string.Equals(xValueElement.Name.LocalName, "struct", StringComparison.OrdinalIgnoreCase))
                {
                    return this.ParseStruct(xValueElement);
                }

                return this.ParseElementValue(xValueElement);
            }
            else
            {
                this.logger.Warning("Value element does not exist.");
            }

            return null;
        }

        private XmlRpcArray ParseArray(XElement xArray)
        {
            var xData = xArray.Element("data");
            if (xData != null)
            {
                var array = new XmlRpcArray();

                foreach (XElement xValue in xData.Elements("value"))
                {
                    var entity = this.ParseValue(xValue);
                    if (entity != null)
                    {
                        array.Entities.Add(entity);
                    }
                }

                return array;
            }
            else
            {
                this.logger.Warning("Array does not have data element.");
            }

            return null;
        }

        private XmlRpcStruct ParseStruct(XElement xStruct)
        {
            var s = new XmlRpcStruct();

            foreach (XElement xMember in xStruct.Elements("member"))
            {
                var xName = xMember.Element("name");
                if (xName != null)
                {
                    var value = this.ParseValue(xMember.Element("value"));
                    if (value != null)
                    {
                        s.Members.Add(xName.Value, value);
                    }
                    else
                    {
                        this.logger.Warning("Cannot get value for member element.");
                    }
                }
                else
                {
                    this.logger.Warning("Member element does not have name.");
                }
            }

            return s;
        }

        private XmlRpcValue ParseElementValue(XElement xValue)
        {
            if (string.Equals(xValue.Name.LocalName, "string", StringComparison.OrdinalIgnoreCase))
            {
                return new XmlRpcValue(xValue.Value);
            }
            else
            {
                this.logger.Error("Values with type '{0}' are not supported.", xValue.Name.LocalName);
            }

            return null;
        }

        private XElement ParameterToXml(object parameter)
        {
            var xValue = new XElement("value");

            if (parameter != null)
            {
                if (parameter.GetType() == TypeString)
                {
                    xValue.Add(new XElement("string", new XText(parameter.ToString())));
                }
            }

            return xValue;
        }
    }
}