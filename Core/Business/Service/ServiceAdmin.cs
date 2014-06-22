using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Common;

namespace Business.Core.Service
{
    /// <summary>
    /// 服务:系统管理员
    /// </summary>
    /// <see cref="http://msdn.microsoft.com/en-us/data/jj592907.aspx" title="Writing SQL queries for entities"/>
    /// <see cref="http://www.entityframeworktutorial.net/EntityFramework4.3/raw-sql-query-in-entity-framework.aspx" title="Execute Native SQL Query"/>
    public class ServiceAdmin : ServiceBasic,IServiceAdmin
    {
        #region 基本函数
        /// <summary>
        /// 保存系统管理员
        /// </summary>
        /// <param name="admin">需要保存的系统管理员信息</param>
        /// <returns></returns>
        public int Save(Admin admin)
        {
            db.Admin.Add(admin);
            db.SaveChanges();
            return (int) admin.ID;
        }

        /// <summary>
        /// 更新系统管理员
        /// </summary>
        /// <param name="admin">需要更新的系统管理员信息</param>
        /// <returns>是否保存成功</returns>
        public bool Update(Admin admin)
        {
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 保存或更新系统管理员
        /// </summary>
        /// <param name="admin">需保存或更新的系统管理员信息</param>
        /// <returns>是否保存或更新成功</returns>
        public bool SaveOrUpdate(Admin admin)
        {
            if (admin.ID==0)
            {
                db.Admin.Add(admin);
            }
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 删除系统管理员
        /// </summary>
        /// <param name="admin">需要删除的系统管理员信息</param>
        /// <returns>是否删除成功</returns>
        public bool Delete(Admin admin)
        {
            db.Admin.Remove(admin);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 由标识删除指定ID数据对象
        /// </summary>
        /// <param name="ID">系统管理员标识</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteByID(int ID)
        {
            Admin admin = db.Admin.Find(ID);
            if (admin != null)
            {
                db.Admin.Remove(admin);
                db.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 根据主键删除多条记录
        /// </summary>
        /// <param name="ids">多个系统管理员标识的字符串:1,2,3,4</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteByIDs(string IDs)
        {
            string[] ID_Arr = IDs.Split(new char[1] {','},StringSplitOptions.RemoveEmptyEntries);
            string Condition="";
            if ((ID_Arr!=null)&&(ID_Arr.Length>0))
            {
                Condition=" ID="+ID_Arr[0]+" ";
                for (int i = 1; i < ID_Arr.Length; i++)
			    {
			        Condition+=" OR ID="+ID_Arr[i]+" ";
			    }
            }
            db.Database.ExecuteSqlCommand("DELETE FROM Admin WHERE " +Condition);
            return true;
        }

        /// <summary>
        /// 由标识判断指定ID系统管理员是否存在
        /// </summary>
        /// <param name="ID">系统管理员标识<></param>
        /// <returns></returns>
        public bool ExistByID(int ID)
        {
            int result = db.Database.SqlQuery<int>("SELECT COUNT(ID) from Admin WHERE ID=" + ID).First();
            return (result > 0);
        }

        /// <summary>
        /// 判断符合条件的数据对象是否存在
        /// </summary>
        /// <param name="WhereClause">查询条件<></param>
        /// <returns></returns>
        public bool ExistBy(string WhereClause = null)
        {
            if (!string.IsNullOrEmpty(WhereClause)) WhereClause = " WHERE "+WhereClause;
            int result = db.Database.SqlQuery<int>("SELECT COUNT(ID) from Admin " + WhereClause).First();
            return (result > 0);
        }

        /// <summary>
        /// 根据表ID主键获取指定的系统管理员[ID对应的表列]
        /// </summary>
        /// <param name="ID">标识</param>
        /// <returns></returns>
        public Admin GetByID(int ID)
        {
            return db.Admin.Find(ID);
        }

        /// <summary>
        /// 获取系统管理员列表
        /// </summary>
        /// <param name="WhereClause"></param>
        /// <param name="OrderBy"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public List<Admin> Get(string WhereClause=null, string OrderBy="ID DESC", int RowCount=10)
        {
            List<Admin> result = null;
            if (!string.IsNullOrEmpty(WhereClause)) WhereClause = " WHERE " + WhereClause;
            var admins = db.Admin.SqlQuery("SELECT TOP " + RowCount + " * FROM Admin " + WhereClause + " ORDER BY " + OrderBy);
            result = admins.ToList<Admin>();
            return result;
        }
        
        /// <summary>
        /// 查询得到单个系统管理员
        /// </summary>
        /// <param name="WhereClause"></param>
        /// <param name="OrderBy"></param>
        /// <param name="RowCount"></param>
        /// <returns></returns>
        public Admin GetOne(string WhereClause = null, string OrderBy = "ID DESC")
        {
            Admin result = null;
            if (!string.IsNullOrEmpty(WhereClause)) WhereClause = " WHERE " + WhereClause;
            var admins = db.Admin.SqlQuery("SELECT TOP 1 * FROM Admin " + WhereClause + " ORDER BY " + OrderBy);
            result = admins.FirstOrDefault();
            return result;
        }
        
        /// <summary>
        /// 总计数
        /// </summary>
        /// <param name="WhereClause">查询条件<></param>
        /// <returns></returns>
        public int Count(string WhereClause=null)
        {
            if (!string.IsNullOrEmpty(WhereClause)) WhereClause = " WHERE " + WhereClause;
            int result = db.Database.SqlQuery<int>("SELECT COUNT(ID) FROM Admin " + WhereClause).First();
            return result;
        }

        /// <summary>
        /// 分页方法:查询系统管理员分页
        /// </summary>
        /// <see cref="http://www.cnblogs.com/ddlink/archive/2013/03/30/2991007.html" comment="采用第五种方法" title="真正高效的SQLSERVER分页查询"/>
        /// <param name="StartPoint">分页开始记录数</param>
        /// <param name="EndPoint">分页结束记录数</param>
        /// <param name="WhereClause">查询条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns></returns>
        public List<Admin> QueryPage(int StartPoint, int EndPoint, string WhereClause=null, string OrderBy="ID DESC")
        {
            List<Admin> result = null;
            if (StartPoint >= 1) StartPoint -= 1;
            if (StartPoint > EndPoint) StartPoint = EndPoint;
            if (!string.IsNullOrEmpty(WhereClause)) WhereClause = " WHERE "+WhereClause;
            string sqlStr = "SELECT w1.* FROM Admin w1,"+
                            "(SELECT TOP " + EndPoint + " row_number() OVER (ORDER BY " + OrderBy + ") n, ID FROM Admin " + WhereClause + ") w2 " +
                            "WHERE w1.ID = w2.ID AND w2.n > " + StartPoint + " ORDER BY w2.n ASC";
            var admins = db.Admin.SqlQuery(sqlStr);
            result = admins.ToList<Admin>();
            return result;
        }

        /// <summary>
        /// 分页方法:根据当前页数和每页显示记录数查询系统管理员分页
        /// </summary>
        /// <param name="PageNo">当前页数</param>
        /// <param name="EndPoint">每页显示记录数</param>
        /// <param name="WhereClause">查询条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns></returns>
        public List<Admin> QueryPageByPageNo(int PageNo, int PageSize, string WhereClause = null, string OrderBy = "ID DESC")
        {
            List<Admin> result = null;

            int StartPoint = (PageNo-1)*PageSize;
            int EndPoint = PageNo * PageSize;
            if (!string.IsNullOrEmpty(WhereClause)) WhereClause = " WHERE " + WhereClause;

            string sqlStr = "SELECT w1.* FROM Admin w1," +
                            "(SELECT TOP " + EndPoint + " row_number() OVER (ORDER BY " + OrderBy + ") n, ID FROM Admin " + WhereClause + ") w2 " +
                            "WHERE w1.ID = w2.ID AND w2.n > " + StartPoint + " ORDER BY w2.n ASC";
            var admins = db.Admin.SqlQuery(sqlStr);
            result = admins.ToList<Admin>();
            return result;
        }

        /// <summary>
        /// 执行SQL语句,返回系统管理员列表
        /// </summary>
        /// <param name="Sqlstr">sql语句</param>
        /// <returns></returns>
        public List<Admin> SqlExecute(string Sqlstr)
        {
            List<Admin> result = null;
            var admins = db.Admin.SqlQuery(Sqlstr);
            result = admins.ToList<Admin>();
            return result;
        }
        #endregion

        #region 统计

        #endregion
    }
}
