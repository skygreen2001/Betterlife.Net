Ext.namespace("BetterlifeNet.Admin");
Bn = BetterlifeNet.Admin;
/**
 * 页面布局格局
 */
Bn.Layout =
{
    /**
     * 布局：页面头部
     */
    HeaderPanel: [{
        region: 'north', ref: 'head', header: false, collapsible: true, collapseMode: 'mini', split: true, height: 27*3,//contentEl:'header',
        tbar: {
            xtype: 'container', layout: 'anchor',
            height: 27*3 , style: 'font-size:14px',
            items: [
                new Ext.Toolbar({
                    height: 27, ref: 'menus',
                    items: [
                        {
                            text: '', ref: '../../file', iconCls: 'logo',
                            menu: {
                                xtype: 'menu', items: [
                                  /*{text:'新建',handler:function(){}},
                                    {text:'导入',handler:function(){}},
                                    {text:'导出',handler:function(){}},*/
                                    {
                                        text: '关闭所有', handler: function () {
                                            Bn.Viewport.center.tabCloseMenu.onCloseAll();
                                        }
                                    }, '-',
                                    {
                                        text: '退出', iconCls: 'icon-quit', ref: 'exit', handler: function () {
                                            window.location.href = "/Index/Logout";
                                        }
                                    }
                                ]
                            }
                        }, {
                            text: '显示', ref: '../../view', iconCls: 'page', handler: function () {
                                var headerPanel = this.ownerCt.ownerCt.ownerCt;
                                if (window.outerHeight == screen.height && window.outerWidth == screen.width) {
                                    headerPanel.view.menu.full.setChecked(true);
                                } else {
                                    headerPanel.view.menu.full.setChecked(false);
                                }
                            }, menu: {
                                xtype: 'menu', items: [
                                    '-',
                                    {
                                        text: '工具栏', checked: true, ref: 'toolbar', checkHandler: function () {
                                            if (Bn.Viewport.head.view.menu.toolbar.checked) {
                                                Bn.Viewport.head.topToolbar.toolbar.show();
                                                Bn.Viewport.head.topToolbar.setHeight(27 * 3);
                                                Bn.Viewport.head.setHeight(27 * 3);
                                            } else {
                                                Bn.Viewport.head.topToolbar.toolbar.hide();
                                                Bn.Viewport.head.topToolbar.setHeight(27);
                                                Bn.Viewport.head.setHeight(27);
                                            }
                                            Bn.Viewport.head.syncHeight();
                                            Bn.Viewport.doLayout();
                                        }
                                    },
                                    {
                                        text: '导航栏', checked: true, ref: 'nav', checkHandler: function () {
                                            if (Bn.Viewport.head.view.menu.nav.checked) {
                                                Bn.Viewport.west.expand();
                                            } else {
                                                Bn.Viewport.west.collapse();
                                            }
                                        }
                                    },
                                    {
                                        text: '在线编辑器', ref: 'onlineditor', menu: {
                                            items: [{
                                                text: '默认【ckEditor】',
                                                checked: true, value: "1",
                                                group: 'onlineditor',
                                                checkHandler: function (item, checked) { Bn.Layout.Function.onOnlineditorCheck(item, checked); }
                                            }, {
                                                text: 'KindEditor',
                                                checked: false, value: "2",
                                                group: 'onlineditor',
                                                checkHandler: function (item, checked) { Bn.Layout.Function.onOnlineditorCheck(item, checked); }
                                            }, {
                                                text: 'xHeditor',
                                                checked: false, value: "3",
                                                group: 'onlineditor',
                                                checkHandler: function (item, checked) { Bn.Layout.Function.onOnlineditorCheck(item, checked); }
                                            }]
                                        }
                                    },
                                    '-',
                                    {
                                        text: '全屏  [F11]', checked: false, ref: 'full', checkHandler: function () {
                                            Bn.Layout.Function.FullScreen();
                                        }
                                    }
                                ]
                            }
                        },{
                            text: '帮助', ref: '../../help', iconCls: 'page',
                            menu: {
                                xtype: 'menu', items: [
                                    { text: '帮助手册', handler: function () { } },
                                    { text: '在线帮助', handler: function () { } },
                                    { text: '在线升级', handler: function () { } },
                                    { text: '关于...', handler: function () { } }
                                ]
                            }
                        }, new Ext.Toolbar.Fill(), '-', {
                            text: '退出', iconCls: 'icon-quit', ref: '../../exit', handler: function () {
                                window.location.href = "/Index/Logout";
                            }
                        }]
                }),
                new Ext.Toolbar({
                    height: 54, ref: 'toolbar',
                    items: [
                        {
                            xtype: 'buttongroup', title: '博客管理', columns: 2, defaults: { scale: 'small' },
                            items: [{
                                text: '添加', iconCls: 'page', ref: "../addBlog", handler: function () {
                                    Bn.Navigation.AddTabbyUrl(Bn.Layout.CenterPanel, "博客", "index.php?go=admin.betterlife.blog", "blog");
                                }
                            },
                               {
                                   text: '管理', iconCls: 'page', ref: "../blogs", handler: function () {
                                       Bn.Navigation.AddTabbyUrl(Bn.Layout.CenterPanel, "博客", "index.php?go=admin.betterlife.blog", "blog");
                                   }
                               }
                            ]
                        }]
                })
            ]
        }
    }],
	/**
	 * 页面左侧
	 */
	LeftPanel : new Ext.Panel({
		region : 'west',
		ref: 'west',
		title : '功能区',
		collapseMode : 'mini',
		split : true,
		width : 200,
		minSize : 100,
		maxSize : 400,
		collapsible : true,
		margins : '0 0 0 5',
		layout : {
			type : 'accordion',
			animate : true//,
			//activeOnTop: true
		}
	}),
    /**
	 * 左侧菜单(超级管理员)
	 */
	LeftMenuGroups: [
       {
           contentEl: 'systemNav',
           title: '系统管理',
           border: false,
           iconCls: 'systemNav'
        },{
            contentEl: 'devNav',
            title: '开发者园地',
            border: false,
            iconCls: 'devNav'
        }, {
            contentEl: 'showNav',
            title: '展示区',
            border: false,
            iconCls: 'showNav'
        }, {
            contentEl: 'schoolNav',
            title: '教学区',
            border: false,
            iconCls: 'schoolNav'
        } 
	],
	/**
	 * 页面内容区
	 */
	CenterPanel : new Ext.TabPanel({
		region : 'center', // a center region is ALWAYS
		// required for border layout
		ref: 'center',
		deferredRender : false,
		activeTab : 0, // first tab initially active
		resizeTabs : true, // turn on tab resizing
		minTabWidth : 115,
		tabWidth : 135,
		enableTabScroll : true,
		defaults : {
			autoScroll : true
		},
		listeners:{
			render: function() {
				Ext.getBody().on("contextmenu", Ext.emptyFn, null, {preventDefault: true});
			},
			tabchange: function(tabPanel, tab){
				if (tab){
					//tabs切换时修改浏览器hash
				    Ext.History.add(tabPanel.id + Bn.Config.TokenDelimiter + tab.id);
				}
			},
			afterrender: function () {
			    //Bn.Navigation.addTabByUrl(Ext.getCmp('centerPanel'), "本地商品管理", "/View/Page/Product.aspx", "Product");
			}
		},
		items: [{
		    title: '首页',
		    contentEl: 'centerArea',
		    iconCls: 'tabs',
		    autoScroll: true
		}],
		plugins : [new Ext.ux.TabCloseMenu(),// Tab标题头右键菜单选项生成
			new Ext.ux.TabScrollerMenu({maxText : 15,pageSize : 5})// Tab标题头右侧下拉选择指定页面
		]
	}),
    /**
     * 布局:页面内容区,无标题栏
     */
	CenterPanel_NoTabs: [{
	    region: 'center', ref: 'center',
	    contentEl: 'centerArea',
	    height: '100%',
	    layout: 'fit',
	    title: '',
	    listeners: {
	        render: function () {
	            Ext.getBody().on("contextmenu", Ext.emptyFn, null, { preventDefault: true });
	        }
	    },
	    margins: '0 3 3 0'
	}],
    /**
     * 布局:页面右部
     */
	RightPanel: [{
	    region: 'east', title: '', collapsible: true, collapseMode: 'mini', split: true,
	    width: 225, minSize: 175, maxSize: 400, margins: '0 5 0 0', layout: 'fit',
	    items: new Ext.TabPanel({
	        border: false, activeTab: 1, tabPosition: 'bottom',
	        items: [{
	            html: '<p>技术人员CMS框架的最爱.</p>',
	            title: 'BetterLife',
	            autoScroll: true
	        }, new Ext.grid.PropertyGrid({
	            title: '属性',
	            closable: true,
	            source: {
	                '(名称)': 'BetterLifeNet',
	                '友好': true,
	                '专业': true,
	                '专注': true,
	                '创建时间': new Date(Date.parse('04/05/2014')),
	                '邮箱': 'skygreen2001@gmail.com',
	                '版本': 1.0,
	                '介绍': '提供专业服务、并向企业客户提供IT服务为重点的盈利公司。'
	            }
	        })
	        ]
	    })
	}],
    /**
     * 布局:页面底部
     */
	FooterPanel: [{
	    region: 'south', contentEl: 'south', collapsible: true,
	    title: '状态栏', split: true, collapseMode: 'mini', layout: 'fit', autoScroll: true,
	    height: 50, minSize: 100, maxSize: 200, margins: '0 0 0 0'
	}],
	/**
	 * Layout初始化
	 */
	Init: function () {
	    Bn.Layout.LeftMenuGroups = Bn.Layout.LeftMenuGroups;
	    Bn.Viewport.west.add(Bn.Layout.LeftMenuGroups);
	    Bn.Viewport.west.doLayout();
	    if (Bn.Viewport.layout.north) {
	        Bn.Viewport.layout.north.split.el.dom.style.cursor = "inherit";
	        Bn.Viewport.layout.north.split.dd.lock();
		}
	    var navEs = Bn.Viewport.west.el.select('a');
	    navEs.on('click', Bn.Navigation.HyperlinkClicked);
	    navEs.on('contextmenu', Bn.Navigation.OnContextMenu);
	    if (Bn.Viewport.head) {
	        if (Bn.Viewport.head.operator) Bn.Viewport.head.operator.setText(Bn.Config.Operator);
	        //设置当前在线编辑器的菜单选项
	        if (Bn.Viewport.head.view) {
	            var onlineditorItems = Bn.Viewport.head.view.menu.onlineditor.menu.items.items;
	            Ext.each(onlineditorItems, function (item) {
	                //console.log(item.value);
	                if (item.value == Bn.Config.OnlineEditor) {
	                    item.checked = true;
	                } else {
	                    item.checked = false;
	                }
	            });
	        }
	        Bn.Viewport.head.view.menu.toolbar.setChecked(false);
	    }
	},
	Function:{
		onOnlineditorCheck:function(item, checked){
			if (checked){
				Ext.util.Cookies.set('OnlineEditor',item.value);
			}
		},
		/**
		 * 在主界面打开窗口
		 * 参数有两个，默认为可重复打开窗口，当第二个参数为true，始终只打开一个窗口
		 */
		OpenWindow:function(url){
			var IsOnlyOneWindow=true;
			if (arguments[1]==false)IsOnlyOneWindow=false;
			if (IsOnlyOneWindow){
				url=url+"&ow=true";
			}
			Ext.get("frmview").dom.src=url;
		},
		/**
		 * 全屏模式支持:
		 * 支持HTML5,Firefor和Chrome高级版本支持
		 * http://css.dzone.com/articles/pragmatic-introduction-html5
		 * https://developer.mozilla.org/en/DOM/Using_full-screen_mode*/
		FullScreen:function(){
			var isIEPrompt=true;
			if (arguments[0]==false)isIEPrompt=false;
			if (Bn.Viewport.head.view.menu.full.checked) {
				var docElm = document.documentElement;
				if (docElm.requestFullscreen) {
				   docElm.requestFullscreen();
				}
				else if (docElm.mozRequestFullScreen) {
				   docElm.mozRequestFullScreen();
				}
				else if (docElm.webkitRequestFullScreen) {
				   docElm.webkitRequestFullScreen();
				}
				else if (typeof window.ActiveXObject !== "undefined"){
					try
					{
						var wscript = new ActiveXObject("WScript.Shell");
						if (wscript !== null) {
							wscript.SendKeys("{F11}");
						}
					}
					catch(err)
					{
					   if(!((window.outerHeight==screen.height && window.outerWidth==screen.width))){
						   if (isIEPrompt){
								Ext.Msg.alert('提示', 'IE浏览器请使用快捷键:F11');
						   }
						   Bn.Layout.HeaderPanel.view.menu.full.setChecked(false);
						   return;
					   }
					}
				}
			}else{
				if (document.exitFullscreen) {
				   document.exitFullscreen();
				}
				else if (document.mozCancelFullScreen) {
				   document.mozCancelFullScreen();
				}
				else if (document.webkitCancelFullScreen) {
					document.webkitCancelFullScreen();
				}else{
					try
					{
						var wscript = new ActiveXObject("WScript.Shell");
						if (wscript !== null) {
							wscript.SendKeys("{F11}");
						}
					}
					catch(err)
					{

						if((window.outerHeight==screen.height && window.outerWidth==screen.width)){
							if (isIEPrompt){
								Ext.Msg.alert('提示', 'IE浏览器请使用快捷键:F11');
							}
						}
						return;
					}
				}
			}
		}
	}
};