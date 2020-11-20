using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Access.Primitives.Extensions.Extensions;
using Dapper;

namespace Access.Primitives.EFCore
{
    public class RLSItems : Dictionary<string, string>
    {

        private static Func<string, string> SqlTemplate = (key) => $"exec sp_set_session_context @key = N'{key}', @value = @{key}";
        public string RegisteredUserId
        {
            get { return this[nameof(RegisteredUserId)]; }
            set
            {
                this.AddOrUpdate(nameof(RegisteredUserId), value, (old, @new) => @new);
            }
        }

        public string CompanyId
        {
            get { return this[nameof(CompanyId)]; }
            set
            {
                this.AddOrUpdate(nameof(CompanyId), value, (old, @new) => @new);
            }
        }
        public string WorkspaceOrganisationId
        {
            get
            {
                return this[nameof(WorkspaceOrganisationId)];
            }
            set
            {
                this.AddOrUpdate(nameof(WorkspaceOrganisationId), value, (old, @new) => @new);
            }
        }

        public string EmployeeId
        {
            get { return this[nameof(EmployeeId)]; }
            set
            {
                this.AddOrUpdate(nameof(EmployeeId), value, (old, @new) => @new);
            }
        }

        public string ToSql()
        {
            return string.Join(Environment.NewLine, this.Select(p => SqlTemplate(p.Key)));
        }

        public SqlMapper.IDynamicParameters ToParameters()
        {
            var args = new DynamicParameters();
            foreach (var arg in this)
            {
                args.Add(arg.Key, arg.Value);
            }
            return args;
        }

        private static RLSItems _empty = new RLSItems();
        public static RLSItems Empty => _empty;

    }
}
