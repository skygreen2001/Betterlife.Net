using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Core.Service
{
    /// <summary>
    /// 服务:系统管理员
    /// </summary>
    /// <see cref="http://msdn.microsoft.com/en-us/data/jj592907.aspx" title="Writing SQL queries for entities"/>
    /// <see cref="http://www.entityframeworktutorial.net/EntityFramework4.3/raw-sql-query-in-entity-framework.aspx" title="Execute Native SQL Query"/>
    public interface IServiceAdmin
    {
        /// <summary>
        /// 保存系统管理员
        /// </summary>
        /// <param name="admin">需要保存的系统管理员信息</param>
        /// <returns></returns>
        int Save(Admin admin);
        /// <summary>
        /// 更新系统管理员
        /// </summary>
        /// <param name="admin">需要更新的系统管理员信息</param>
        /// <returns>是否保存成功</returns>
        bool Update(Admin admin);
        /// <summary>
        /// 保存或更新系统管理员
        /// </summary>
        /// <param name="admin">需保存或更新的系统管理员信息</param>
        /// <returns>是否保存或更新成功</returns>
        bool SaveOrUpdate(Admin admin);
        /// <summary>
        /// 删除系统管理员
        /// </summary>
        /// <param name="admin">需要删除的系统管理员信息</param>
        /// <returns>是否删除成功</returns>
        bool Delete(Admin admin);
        /// <summary>
        /// 由标识删除指定ID数据对象
        /// </summary>
        /// <param name="ID">系统管理员标识</param>
        /// <returns>是否删除成功</returns>
        bool DeleteByID(int ID);
        /// <summary>
        /// 根据主键删除多条记录
        /// </summary>
        /// <param name="ids">多个系统管理员标识的字符串:1,2,3,4</param>
        /// <returns>是否删除成功</returns>
        bool DeleteByIDs(string IDs);
        /// <summary>
        /// 由标识判断指定ID系统管理员是否存在
        /// </summary>
        /// <param name="ID">系统管理员标识<></param>
        /// <returns></returns>
        bool ExistByID(int ID);
        /// <summary>
        /// 判断符合条件的数据对象是否存在
        /// </summary>
        /// <param name="WhereClause">查询条件<></param>
        /// <returns></returns>
        bool ExistBy(string WhereClause = null);
        /// <summary>
        /// 根据表ID主键获取指定的系统管理员[ID对应的表列]
        /// </summary>
        /// <param name="ID">标识</param>
        /// <returns></returns>
        Admin GetByID(int ID);
        /// <summary>
        /// 获取系统管理员列表
        /// </summary>
        /// <param name="WhereClause"></param>
        /// <param name="OrderBy"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        List<Admin> Get(string WhereClause=null, string OrderBy="ID DESC", int RowCount=10);
        /// <summary>
        /// 查询得到单个系统管理员
        /// </summary>
        /// <param name="WhereClause"></param>
        /// <param name="OrderBy"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        Admin GetOne(string WhereClause = null, string OrderBy = "ID DESC");
        /// <summary>
        /// 总计数
        /// </summary>
        /// <param name="WhereClause">查询条件<></param>
        /// <returns></returns>
        int Count(string WhereClause=null);
        /// <summary>
        /// 分页方法:查询系统管理员分页
        /// </summary>
        /// <param name="StartPoint">分页开始记录数</param>
        /// <param name="EndPoint">分页结束记录数</param>
        /// <param name="WhereClause">查询条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns></returns>
        List<Admin> QueryPage(int StartPoint, int EndPoint, string WhereClause = null, string OrderBy = "ID DESC");
        /// <summary>
        /// 分页方法:根据当前页数和每页显示记录数查询系统管理员分页
        /// </summary>
        /// <param name="PageNo">当前页数</param>
        /// <param name="EndPoint">每页显示记录数</param>
        /// <param name="WhereClause">查询条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns></returns>
        List<Admin> QueryPageByPageNo(int PageNo, int PageSize, string WhereClause = null, string OrderBy = "ID DESC");
        /// <summary>
        /// 执行SQL语句,返回系统管理员列表
        /// </summary>
        /// <param name="Sqlstr">sql语句</param>
        /// <returns></returns>
        List<Admin> SqlExecute(string Sqlstr);
    }
}
