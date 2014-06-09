using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.View.OnlineEditor
{
    /// <summary>
    /// 工具类：UEditor
    /// </summary>
    public static class UtilUEditor
    {
        /// <summary>
        /// 设置标准toolbar
        /// </summary>
        /// <returns>配置工具栏</returns>
        public static string Toolbar_Normal()
        {
            return @"[
					[ 

                      'fontfamily', 'fontsize', 'paragraph', 'forecolor', 'backcolor','bold', 'italic', 'underline', 'fontborder', 'strikethrough','|',
                      'lineheight', 'indent', 'touppercase', 'tolowercase','superscript', 'subscript','insertorderedlist', 'insertunorderedlist', '|',
                    ],
                    [ 'link', 'unlink','simpleupload', 'insertimage', 'emotion', 'scrawl', 'insertvideo', 'music', 'attachment', 'map','spechars','wordimage','|',
            		  'undo','redo', 'removeformat', 'formatmatch', 'autotypeset','background','template','snapscreen','preview', 'searchreplace','source','fullscreen'
            		]
            
				]";
        }

        /// <summary>
        /// 初始化，加载基本JS文件
        /// 预加载UEditor的JS函数
        /// </summary>
        /// <returns></returns>
        public static string Init_Function(string Textarea_ID,string ConfigString="")
        {
            bool Is_Toolbar_Full = false;
            string result = "";
            if (Is_Toolbar_Full)
            {
                string jsTemplate= @"
            <script type=""text/javascript"">
            var ue_{0};
            function pageInit_ue_{0}()
            {{
	            ue_{0}=UE.getEditor('{0}');
            }}
            </script>
                ";
                result = string.Format(jsTemplate, Textarea_ID);
            }
            else
            {
			    if (string.IsNullOrEmpty(ConfigString)){
				    ConfigString=Toolbar_Normal();
			    }
                string jsTemplate = @"
            <script type=""text/javascript"">
            var ue_{0};
            function pageInit_ue_{0}()
            {{
	            ue_{0}=UE.getEditor('{0}',{{
		            toolbars:{1}
	            }});
            }}
            </script>
                ";
                result = string.Format(jsTemplate, Textarea_ID, ConfigString);
            }
            
            return result;
        }
    }
}
