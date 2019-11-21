using System;
using System.Data;
using Dapper;
using Newtonsoft.Json;

namespace Dka.AspNetCore.BasicWebApp.Common.UnitTests.RepositoryTests
{
    public class TestGuidTypeHandler : SqlMapper.ITypeHandler
    {
        public object Parse(Type destinationType, object value)
        {
            return new Guid(value.ToString());
        }

        public void SetValue(IDbDataParameter parameter, object value)
        {
            parameter.Value = value?.ToString() ?? (object)DBNull.Value;
            parameter.DbType = DbType.String;
        }   
    }
}