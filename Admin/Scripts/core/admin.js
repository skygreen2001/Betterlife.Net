Ext.namespace("BetterlifeNet.Admin");
Bn = BetterlifeNet.Admin;
Bn.Admin = {
    /**
	 * 全局配置
	 */
    Config: {
        /**
		 *分页:每页显示记录数
		 */
        PageSize: 3,
        /**
		 *显示配置
		 */
        View: {
            /**
			 * 显示系统管理人员的视图相对系统管理人员列表Grid的位置
			 * 1:上方,2:下方,3:左侧,4:右侧,
			 */
            Direction: 2,
            /**
			 *是否显示。
			 */
            IsShow: 0,
            /**
			 * 是否固定显示系统管理人员信息页(或者打开新窗口)
			 */
            IsFix: 0
        },
        /**
         * 在线编辑器类型。
         * 1:CkEditor,2:KindEditor,3:xhEditor
         * 配合Action的变量配置$online_editor
         */
        OnlineEditor: 1
    },
    /**
	 * Cookie设置
	 */
    Cookie: new Ext.state.CookieProvider(),
    /**
	 * 初始化
	 */
    Init: function () {
        if (Bn.Admin.Cookie.get('View.Direction')) {
            Bn.Admin.Config.View.Direction = Bn.Admin.Cookie.get('View.Direction');
        }
        if (Bn.Admin.Cookie.get('View.IsFix') != null) {
            Bn.Admin.Config.View.IsFix = Bn.Admin.Cookie.get('View.IsFix');
        }
        if (Ext.util.Cookies.get('OnlineEditor') != null) {
            Bn.Admin.Config.OnlineEditor = parseInt(Ext.util.Cookies.get('OnlineEditor'));
        }
    }
};
/**
 * Model:数据模型
 */
Bn.Admin.Store = {
    /**
	 * 系统管理人员
	 */
    adminStore: new Ext.data.Store({
        reader: new Ext.data.JsonReader({
            totalProperty: 'totalCount',
            successProperty: 'success',
            root: 'data', remoteSort: true,
            fields: [
                { name: 'ID', type: 'int' },
                { name: 'Department_ID', type: 'int' },
                { name: 'Department_Name', type: 'string' },
                { name: 'Username', type: 'string' },
                { name: 'Realname', type: 'string' },
                { name: 'Password', type: 'string' },
                { name: 'RoletypeShow', type: 'string' },
                { name: 'Roletype', type: 'string' },
                { name: 'SeescopeShow', type: 'string' },
                { name: 'Seescope', type: 'string' },
                { name: 'LoginTimes', type: 'int' }
            ]
        }
		),
        writer: new Ext.data.JsonWriter({
            encode: false
        }),
        listeners: {
            beforeload: function (store, options) {
                if (Ext.isReady) {
                    if (!options.params.limit) options.params.limit = Bn.Admin.Config.PageSize;
                    Ext.apply(options.params, Bn.Admin.View.Running.adminGrid.filter);//保证分页也将查询条件带上
                }
            }
        }
    }),
    /**
     * 用户所属部门
     */
    departmentStoreForCombo:new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: 'HttpData/Core/Department.ashx'
        }),
        reader: new Ext.data.JsonReader({
            root: 'departments',
            autoLoad: true,
            totalProperty: 'totalCount',
            idProperty: 'ID'
        }, [
            {name: 'ID', mapping: 'ID'},
            {name: 'department_name', mapping: 'department_name'}
        ])
    })
};
/**
 * View:系统管理人员显示组件
 */
Bn.Admin.View = {
    /**
	 * 编辑窗口：新建或者修改系统管理人员
	 */
    EditWindow: Ext.extend(Ext.Window, {
        constructor: function (config) {
            config = Ext.apply({
                /**
				 * 自定义类型:保存类型
				 * 0:保存窗口,1:修改窗口
				 */
                savetype: 0,
                closeAction: "hide",
                constrainHeader: true, maximizable: true, collapsible: true,
                width: 450, height: 550, minWidth: 400, minHeight: 450,
                layout: 'fit', plain: true, buttonAlign: 'center',
                defaults: {
                    autoScroll: true
                },
                listeners: {
                    beforehide: function () {
                        this.editForm.form.getEl().dom.reset();
                        this.editForm.form.clearInvalid();
                    }
                },
                items: [
					new Ext.form.FormPanel({
					    ref: 'editForm', layout: 'form',
					    labelWidth: 100, labelAlign: "center",
					    bodyStyle: 'padding:5px 5px 0', align: "center",
					    api: {},
					    defaults: {
					        xtype: 'textfield', anchor: '98%'
					    },
					    items: [
                            { xtype: 'hidden', name: 'ID', ref: '../ID' },
                            { xtype: 'hidden', name: 'Department_ID', ref: '../Department_ID' },
                            { fieldLabel: '用户名', name: 'Username' },
                            { fieldLabel: '真实姓名', name: 'Realname' },
                            { fieldLabel: '密码(<font color=red>*</font>)', name: 'Password', inputType: 'Password', ref: '../Password' },
                            { xtype: 'hidden', name: 'Password_old', ref: '../Password_old' },
                            {
                                fieldLabel: '扮演角色', hiddenName: 'Roletype', xtype: 'combo', ref: '../Roletype',
                                mode: 'local', triggerAction: 'all', lazyRender: true, editable: false, allowBlank: false,
                                store: new Ext.data.SimpleStore({
                                    fields: ['value', 'text'],
                                    data: [['0', '超级管理员'], ['1', '管理人员'], ['2', '运维人员'], ['3', '合作伙伴']]
                                }), emptyText: '请选择扮演角色',
                                valueField: 'value', displayField: 'text'
                            },
                            {
                                fieldLabel: '视野', hiddenName: 'Seescope', xtype: 'combo', ref: '../Seescope',
                                mode: 'local', triggerAction: 'all', lazyRender: true, editable: false, allowBlank: false,
                                store: new Ext.data.SimpleStore({
                                    fields: ['value', 'text'],
                                    data: [['0', '只能查看自己的信息'], ['1', '查看所有的信息']]
                                }), emptyText: '请选择视野',
                                valueField: 'value', displayField: 'text'
                            }
					    ]
					})
                ],
                buttons: [{
                    text: "", ref: "../saveBtn", scope: this,
                    handler: function () {
                        var editwindow = Bn.Admin.View.Running.edit_window;
                        if (!this.editForm.getForm().isValid()) {
                            return;
                        }
                        editWindow = this;
                        if (this.savetype == 0) {
                            this.editForm.api.submit = ExtServiceAdmin.save;
                            this.editForm.getForm().submit({
                                success: function (form, action) {
                                    Ext.Msg.alert("提示", action.result.msg);
                                    Bn.Admin.View.Running.adminGrid.doSelectAdmin();
                                    form.reset();
                                    editWindow.hide();
                                },
                                failure: function (form, response) {
                                    Ext.Msg.show({ title: '提示', width: 350, buttons: { yes: '确定' }, msg: response.result.msg });
                                }
                            });
                        } else {
                            this.editForm.api.submit = ExtServiceAdmin.update;
                            this.editForm.getForm().submit({
                                success: function (form, action) {
                                    Bn.Admin.View.Running.adminGrid.store.reload();
                                    Ext.Msg.show({
                                        title: '提示', msg: '修改成功！', buttons: { yes: '确定' }, fn: function () {
                                            Bn.Admin.View.Running.adminGrid.bottomToolbar.doRefresh();
                                        }
                                    });
                                    form.reset();
                                    editWindow.hide();
                                },
                                failure: function (form, response) {
                                    Ext.Msg.show({ title: '提示', width: 350, buttons: { yes: '确定' }, msg: response.result.msg });
                                }
                            });
                        }
                    }
                }, {
                    text: "取 消", scope: this,
                    handler: function () {
                        this.hide();
                    }
                }, {
                    text: "重 置", ref: '../resetBtn', scope: this,
                    handler: function () {
                        var sel = Bn.Admin.View.Running.adminGrid.getSelectionModel().getSelected();
                        this.editForm.form.loadRecord(sel);
                    }
                }]
            }, config);
            Bn.Admin.View.EditWindow.superclass.constructor.call(this, config);
        },
        /**
         * 查找用户
         */
        getStaff: function () {
            var userName = Bn.Admin.View.Running.edit_window.StaffName.Realname.getValue();
            if (!userName) {
                Ext.Msg.alert('提示', '<font color=red>请先输入用户名！</font>');
                return false;
            }
            var invocStaff = Ext.getCmp("invocationStaff");
            invocStaff.setText("正在获取用户<img src='/View/Images/ico/loading.gif' align='absbottom'/>");
            ExtServiceAdmin.getStaff(userName, function (provider, response) {
                if (response.result) {
                    if (response.result.success) {
                        var info = response.result.info;
                        var editwindow = Bn.Admin.View.Running.edit_window;
                        editwindow.Username.setValue(info.Username);
                    } else {
                        Ext.Msg.alert('提示', "<font color=red>" + response.result.msg + "</font>");
                    }
                } else {
                    Ext.Msg.alert('提示', "<font color=red>获取失败,请重试！</font>");
                }
                invocStaff.setText("<font color=green>获取用户</font>");
            });
        }
    }),
    /**
	 * 显示系统管理人员详情
	 */
    AdminView: {
        /**
		 * Tab页：容器包含显示与系统管理人员所有相关的信息
		 */
        Tabs: Ext.extend(Ext.TabPanel, {
            constructor: function (config) {
                config = Ext.apply({
                    region: 'south', collapseMode: 'mini', split: true,
                    activeTab: 1, tabPosition: "bottom", resizeTabs: true,
                    header: false, enableTabScroll: true, tabWidth: 'auto', margins: '0 3 3 0',
                    defaults: {
                        autoScroll: true,
                        layout: 'fit'
                    },
                    listeners: {
                        beforetabchange: function (tabs, newtab, currentTab) {
                            if (tabs.tabFix == newtab) {
                                if (Bn.Admin.View.Running.adminGrid.getSelectionModel().getSelected() == null) {
                                    Ext.Msg.alert('提示', '请先选择系统管理人员！');
                                    return false;
                                }
                                Bn.Admin.Config.View.IsShow = 1;
                                Bn.Admin.View.Running.adminGrid.showAdmin();
                                Bn.Admin.View.Running.adminGrid.tvpView.menu.mBind.setChecked(false);
                                return false;
                            }
                        }
                    },
                    items: [
						{ title: '+', tabTip: '取消固定', ref: 'tabFix', iconCls: 'icon-fix' }
                    ]
                }, config);
                Bn.Admin.View.AdminView.Tabs.superclass.constructor.call(this, config);

                this.onAddItems();
            },
            /**
			 * 根据布局调整Tabs的宽度或者高度以及折叠
			 */
            enableCollapse: function () {
                if ((Bn.Admin.Config.View.Direction == 1) || (Bn.Admin.Config.View.Direction == 2)) {
                    this.width = Ext.getBody().getViewSize().width;
                    this.height = Ext.getBody().getViewSize().height / 2;
                } else {
                    this.width = Ext.getBody().getViewSize().width / 2;
                    this.height = Ext.getBody().getViewSize().height;
                }
                this.ownerCt.setSize(this.width, this.height);
                if (this.ownerCt.collapsed) this.ownerCt.expand();
                this.ownerCt.collapsed = false;
            },
            onAddItems: function () {
                this.add(
					{
					    title: '基本信息', ref: 'tabAdminDetail', iconCls: 'tabs',
					    tpl: [
                             '<table class="viewdoblock">',
                             '    <tr class="entry"><td class="head">部门</td><td class="content">{Department_Name}</td></tr>',
                             '    <tr class="entry"><td class="head">用户名</td><td class="content">{Username}</td></tr>',
                             '    <tr class="entry"><td class="head">真实姓名</td><td class="content">{Realname}</td></tr>',
                             '    <tr class="entry"><td class="head">扮演角色</td><td class="content">{RoletypeShow}</td></tr>',
                             '    <tr class="entry"><td class="head">视野</td><td class="content">{SeescopeShow}</td></tr>',
                             '    <tr class="entry"><td class="head">登录次数</td><td class="content">{LoginTimes}</td></tr>',
                             '</table>'
					    ]
					}
				);
            }
        }),
        /**
		 * 窗口:显示系统管理人员信息
		 */
        Window: Ext.extend(Ext.Window, {
            constructor: function (config) {
                config = Ext.apply({
                    title: "查看系统管理人员", constrainHeader: true, maximizable: true, minimizable: true,
                    width: 705, height: 500, minWidth: 450, minHeight: 400,
                    layout: 'fit', resizable: true, plain: true, bodyStyle: 'padding:5px;',
                    closeAction: "hide",
                    items: [new Bn.Admin.View.AdminView.Tabs({ ref: 'winTabs', tabPosition: 'top' })],
                    listeners: {
                        minimize: function (w) {
                            w.hide();
                            Bn.Admin.Config.View.IsShow = 0;
                            Bn.Admin.View.Running.adminGrid.tvpView.menu.mBind.setChecked(true);
                        },
                        hide: function (w) {
                            Bn.Admin.View.Running.adminGrid.tvpView.toggle(false);
                        }
                    },
                    buttons: [{
                        text: '新增系统管理人员', scope: this,
                        handler: function () { this.hide(); Bn.Admin.View.Running.adminGrid.addAdmin(); }
                    }, {
                        text: '修改系统管理人员', scope: this,
                        handler: function () { this.hide(); Bn.Admin.View.Running.adminGrid.updateAdmin(); }
                    }]
                }, config);
                Bn.Admin.View.AdminView.Window.superclass.constructor.call(this, config);
            }
        })
    },

    /**
	 * 视图：系统管理人员列表
	 */
    Grid: Ext.extend(Ext.grid.GridPanel, {
        constructor: function (config) {
            config = Ext.apply({
                /**
				 * 查询条件
				 */
                filter: null,
                region: 'center',
                store: Bn.Admin.Store.adminStore,
                sm: this.sm,
                frame: true, trackMouseOver: true, enableColumnMove: true, columnLines: true,
                loadMask: true, stripeRows: true, headerAsText: false,
                defaults: {
                    autoScroll: true
                },
                cm: new Ext.grid.ColumnModel({
                    defaults: {
                        width: 120, sortable: true
                    },
                    columns: [
						this.sm,
                        { header: '标识', dataIndex: 'ID', hidden: true },
                        { header: '部门', dataIndex: 'Department_Name' },
                        { header: '用户名', dataIndex: 'Username' },
                        { header: '真实姓名', dataIndex: 'Realname' },
                        { header: '扮演角色', dataIndex: 'RoletypeShow' },
                        { header: '视野', dataIndex: 'SeescopeShow' },
                        { header: '登录次数', dataIndex: 'LoginTimes' }
                    ]
                }),
                tbar: {
                    xtype: 'container', layout: 'anchor', height: 27 * 2, style: 'font-size:14px',
                    defaults: {
                        height: 27, anchor: '100%'
                    },
                    items: [
						new Ext.Toolbar({
						    enableOverflow: true, width: 100,
						    defaults: {
						        xtype: 'textfield',
						        listeners: {
						            specialkey: function (field, e) {
						                if (e.getKey() == Ext.EventObject.ENTER) this.ownerCt.ownerCt.ownerCt.doSelectAdmin();
						            }
						        }
						    },
						    items: [
                                '用户名', '&nbsp;&nbsp;', { ref: '../aUsername' }, '&nbsp;&nbsp;',
                                '真实姓名', '&nbsp;&nbsp;', { ref: '../aRealname' }, '&nbsp;&nbsp;',
								{
								    xtype: 'button', text: '查询', scope: this,
								    handler: function () {
								        this.doSelectAdmin();
								    }
								},
								{
								    xtype: 'button', text: '重置', scope: this,
								    handler: function () {
								        this.topToolbar.aUsername.setValue("");
								        this.topToolbar.aRealname.setValue("");
								        this.filter = {};
								        this.doSelectAdmin();
								    }
								}]
						}),
						new Ext.Toolbar({
						    defaults: { scope: this },
						    items: [
								{
								    text: '反选', iconCls: 'icon-reverse',
								    handler: function () {
								        this.onReverseSelect();
								    }
								}, '-', {
								    text: '添加系统管理人员', iconCls: 'icon-add',
								    handler: function () {
								        this.addAdmin();
								    }
								}, '-', {
								    text: '修改系统管理人员', ref: '../../btnUpdate', iconCls: 'icon-edit', disabled: true,
								    handler: function () {
								        this.updateAdmin();
								    }
								}
                                , '-', {
                                    text: '删除系统管理人员', ref: '../../btnRemove', iconCls: 'icon-delete', disabled: true,
                                    handler: function () {
                                        this.deleteAdmin();
                                    }
                                }
                                , '-', {
                                    xtype: 'tbsplit', text: '查看系统管理人员', ref: '../../tvpView', iconCls: 'icon-updown',
                                    enableToggle: true, disabled: true,
                                    handler: function () { this.showAdmin() },
                                    menu: {
                                        xtype: 'menu', plain: true,
                                        items: [
											{ text: '上方', group: 'mlayout', checked: false, iconCls: 'view-top', scope: this, handler: function () { this.onUpDown(1) } },
											{ text: '下方', group: 'mlayout', checked: true, iconCls: 'view-bottom', scope: this, handler: function () { this.onUpDown(2) } },
											{ text: '左侧', group: 'mlayout', checked: false, iconCls: 'view-left', scope: this, handler: function () { this.onUpDown(3) } },
											{ text: '右侧', group: 'mlayout', checked: false, iconCls: 'view-right', scope: this, handler: function () { this.onUpDown(4) } },
											{ text: '隐藏', group: 'mlayout', checked: false, iconCls: 'view-hide', scope: this, handler: function () { this.hideAdmin(); Bn.Admin.Config.View.IsShow = 0; } }, '-',
											{ text: '固定', ref: 'mBind', checked: true, scope: this, checkHandler: function (item, checked) { this.onBindGrid(item, checked); Bn.Admin.Cookie.set('View.IsFix', Bn.Admin.Config.View.IsFix); } }
                                        ]
                                    }
                                }, '-']
						}
					)]
                },
                bbar: new Ext.PagingToolbar({
                    pageSize: Bn.Admin.Config.PageSize,
                    store: Bn.Admin.Store.adminStore,
                    scope: this, autoShow: true, displayInfo: true,
                    displayMsg: '当前显示 {0} - {1}条记录/共 {2}条记录。',
                    emptyMsg: "无显示数据",
                    listeners: {
                        change: function (thisbar, pagedata) {
                            if (Bn.Admin.Config.View.IsShow == 1) {
                                Bn.Admin.View.IsSelectView = 1;
                            }
                            this.ownerCt.hideAdmin();
                            Bn.Admin.Config.View.IsShow = 0;
                        }
                    },
                    items: [
						{ xtype: 'label', text: '每页显示' },
						{
						    xtype: 'numberfield', value: Bn.Admin.Config.PageSize, minValue: 1, width: 35,
						    style: 'text-align:center', allowBlank: false,
						    listeners:
							{
							    change: function (Field, newValue, oldValue) {
							        var num = parseInt(newValue);
							        if (isNaN(num) || !num || num < 1) {
							            num = Bn.Admin.Config.PageSize;
							            Field.setValue(num);
							        }
							        this.ownerCt.pageSize = num;
							        Bn.Admin.Config.PageSize = num;
							        this.ownerCt.ownerCt.doSelectAdmin();
							    },
							    specialKey: function (field, e) {
							        if (e.getKey() == Ext.EventObject.ENTER) {
							            var num = parseInt(field.getValue());
							            if (isNaN(num) || !num || num < 1) {
							                num = Bn.Admin.Config.PageSize;
							            }
							            this.ownerCt.pageSize = num;
							            Bn.Admin.Config.PageSize = num;
							            this.ownerCt.ownerCt.doSelectAdmin();
							        }
							    }
							}
						},
						{ xtype: 'label', text: '个' }
                    ]
                })
            }, config);
            //初始化显示系统管理人员列表
            this.doSelectAdmin();
            Bn.Admin.View.Grid.superclass.constructor.call(this, config);
            //创建在Grid里显示的系统管理人员信息Tab页
            Bn.Admin.View.Running.viewTabs = new Bn.Admin.View.AdminView.Tabs();
            this.addListener('rowdblclick', this.onRowDoubleClick);
        },
        /**
		 * 行选择器
		 */
        sm: new Ext.grid.CheckboxSelectionModel({
            //handleMouseDown : Ext.emptyFn,
            listeners: {
                selectionchange: function (sm) {
                    // 判断删除和更新按钮是否可以激活
                    this.grid.btnRemove.setDisabled(sm.getCount() < 1);
                    this.grid.btnUpdate.setDisabled(sm.getCount() != 1);
                    this.grid.tvpView.setDisabled(sm.getCount() != 1);
                },
                rowselect: function (sm, rowIndex, record) {
                    this.grid.updateViewAdmin();
                    if (sm.getCount() != 1) {
                        this.grid.hideAdmin();
                        Bn.Admin.Config.View.IsShow = 0;
                    } else {
                        if (Bn.Admin.View.IsSelectView == 1) {
                            Bn.Admin.View.IsSelectView = 0;
                            this.grid.showAdmin();
                        }
                    }
                },
                rowdeselect: function (sm, rowIndex, record) {
                    if (sm.getCount() != 1) {
                        if (Bn.Admin.Config.View.IsShow == 1) {
                            Bn.Admin.View.IsSelectView = 1;
                        }
                        this.grid.hideAdmin();
                        Bn.Admin.Config.View.IsShow = 0;
                    }
                }
            }
        }),
        /**
		 * 双击选行
		 */
        onRowDoubleClick: function (grid, rowIndex, e) {
            if (!Bn.Admin.Config.View.IsShow) {
                this.sm.selectRow(rowIndex);
                this.showAdmin();
                this.tvpView.toggle(true);
            } else {
                this.hideAdmin();
                Bn.Admin.Config.View.IsShow = 0;
                this.sm.deselectRow(rowIndex);
                this.tvpView.toggle(false);
            }
        },
        /**
		 * 是否绑定在本窗口上
		 */
        onBindGrid: function (item, checked) {
            if (checked) {
                Bn.Admin.Config.View.IsFix = 1;
            } else {
                Bn.Admin.Config.View.IsFix = 0;
            }
            if (this.getSelectionModel().getSelected() == null) {
                Bn.Admin.Config.View.IsShow = 0;
                return;
            }
            if (Bn.Admin.Config.View.IsShow == 1) {
                this.hideAdmin();
                Bn.Admin.Config.View.IsShow = 0;
            }
            this.showAdmin();
        },
        /**
		 * 反选
		 */
        onReverseSelect: function () {
            for (var i = this.getView().getRows().length - 1; i >= 0; i--) {
                if (this.sm.isSelected(i)) {
                    this.sm.deselectRow(i);
                } else {
                    this.sm.selectRow(i, true);
                }
            }
        },
        /**
		 * 查询符合条件的系统管理人员
		 */
        doSelectAdmin: function () {
            if (this.topToolbar) {
                var aUsername = this.topToolbar.aUsername.getValue();
                var aRealname = this.topToolbar.aRealname.getValue();
                this.filter = { 'Username': aUsername, 'Realname': aRealname };
            }
            var condition = { 'start': 0, 'limit': Bn.Admin.Config.PageSize };
            Ext.apply(condition, this.filter);
            ExtServiceAdmin.queryPageAdmin(condition, function (provider, response) {
                if (response.result && response.result.data) {
                    var result = new Array();
                    result['data'] = response.result.data;
                    result['totalCount'] = response.result.totalCount;
                    Bn.Admin.Store.adminStore.loadData(result);
                } else {
                    Bn.Admin.Store.adminStore.removeAll();
                    Ext.Msg.alert('提示', '无符合条件的系统管理人员！');
                }
            });
        },
        /**
		 * 显示系统管理人员视图
		 * 显示系统管理人员的视图相对系统管理人员列表Grid的位置
		 * 1:上方,2:下方,0:隐藏。
		 */
        onUpDown: function (viewDirection) {
            Bn.Admin.Config.View.Direction = viewDirection;
            switch (viewDirection) {
                case 1:
                    this.ownerCt.north.add(Bn.Admin.View.Running.viewTabs);
                    break;
                case 2:
                    this.ownerCt.south.add(Bn.Admin.View.Running.viewTabs);
                    break;
                case 3:
                    this.ownerCt.west.add(Bn.Admin.View.Running.viewTabs);
                    break;
                case 4:
                    this.ownerCt.east.add(Bn.Admin.View.Running.viewTabs);
                    break;
            }
            Bn.Admin.Cookie.set('View.Direction', Bn.Admin.Config.View.Direction);
            if (this.getSelectionModel().getSelected() != null) {
                if ((Bn.Admin.Config.View.IsFix == 0) && (Bn.Admin.Config.View.IsShow == 1)) {
                    this.showAdmin();
                }
                Bn.Admin.Config.View.IsFix = 1;
                Bn.Admin.View.Running.adminGrid.tvpView.menu.mBind.setChecked(true, true);
                Bn.Admin.Config.View.IsShow = 0;
                this.showAdmin();
            }
        },
        /**
		 * 显示系统管理人员
		 */
        showAdmin: function () {
            if (this.getSelectionModel().getSelected() == null) {
                Ext.Msg.alert('提示', '请先选择系统管理人员！');
                Bn.Admin.Config.View.IsShow = 0;
                this.tvpView.toggle(false);
                return;
            }
            if (Bn.Admin.Config.View.IsFix == 0) {
                if (Bn.Admin.View.Running.view_window == null) {
                    Bn.Admin.View.Running.view_window = new Bn.Admin.View.AdminView.Window();
                }
                if (Bn.Admin.View.Running.view_window.hidden) {
                    Bn.Admin.View.Running.view_window.show();
                    Bn.Admin.View.Running.view_window.winTabs.hideTabStripItem(Bn.Admin.View.Running.view_window.winTabs.tabFix);
                    this.updateViewAdmin();
                    this.tvpView.toggle(true);
                    Bn.Admin.Config.View.IsShow = 1;
                } else {
                    this.hideAdmin();
                    Bn.Admin.Config.View.IsShow = 0;
                }
                return;
            }
            switch (Bn.Admin.Config.View.Direction) {
                case 1:
                    if (!this.ownerCt.north.items.contains(Bn.Admin.View.Running.viewTabs)) {
                        this.ownerCt.north.add(Bn.Admin.View.Running.viewTabs);
                    }
                    break;
                case 2:
                    if (!this.ownerCt.south.items.contains(Bn.Admin.View.Running.viewTabs)) {
                        this.ownerCt.south.add(Bn.Admin.View.Running.viewTabs);
                    }
                    break;
                case 3:
                    if (!this.ownerCt.west.items.contains(Bn.Admin.View.Running.viewTabs)) {
                        this.ownerCt.west.add(Bn.Admin.View.Running.viewTabs);
                    }
                    break;
                case 4:
                    if (!this.ownerCt.east.items.contains(Bn.Admin.View.Running.viewTabs)) {
                        this.ownerCt.east.add(Bn.Admin.View.Running.viewTabs);
                    }
                    break;
            }
            this.hideAdmin();
            if (Bn.Admin.Config.View.IsShow == 0) {
                Bn.Admin.View.Running.viewTabs.enableCollapse();
                switch (Bn.Admin.Config.View.Direction) {
                    case 1:
                        this.ownerCt.north.show();
                        break;
                    case 2:
                        this.ownerCt.south.show();
                        break;
                    case 3:
                        this.ownerCt.west.show();
                        break;
                    case 4:
                        this.ownerCt.east.show();
                        break;
                }
                this.updateViewAdmin();
                this.tvpView.toggle(true);
                Bn.Admin.Config.View.IsShow = 1;
            } else {
                Bn.Admin.Config.View.IsShow = 0;
            }
            this.ownerCt.doLayout();
        },
        /**
		 * 隐藏系统管理人员
		 */
        hideAdmin: function () {
            this.ownerCt.north.hide();
            this.ownerCt.south.hide();
            this.ownerCt.west.hide();
            this.ownerCt.east.hide();
            if (Bn.Admin.View.Running.view_window != null) {
                Bn.Admin.View.Running.view_window.hide();
            }
            this.tvpView.toggle(false);
            this.ownerCt.doLayout();
        },
        /**
		 * 更新当前系统管理人员显示信息
		 */
        updateViewAdmin: function () {

            if (Bn.Admin.View.Running.view_window != null) {
                Bn.Admin.View.Running.view_window.winTabs.tabAdminDetail.update(this.getSelectionModel().getSelected().data);
            }
            Bn.Admin.View.Running.viewTabs.tabAdminDetail.update(this.getSelectionModel().getSelected().data);
        },
        /**
		 * 新建系统管理人员
		 */
        addAdmin: function () {
            if (Bn.Admin.View.Running.edit_window == null) {
                Bn.Admin.View.Running.edit_window = new Bn.Admin.View.EditWindow();
            }
            var edit_window = Bn.Admin.View.Running.edit_window;
            edit_window.resetBtn.setVisible(false);
            edit_window.saveBtn.setText('保 存');
            edit_window.setTitle('添加系统管理人员');
            edit_window.savetype = 0;
            edit_window.ID.setValue("");

            edit_window.show();
            edit_window.maximize();
        },
        /**
		 * 编辑系统管理人员时先获得选中的系统管理人员信息
		 */
        updateAdmin: function () {
            if (Bn.Admin.View.Running.edit_window == null) {
                Bn.Admin.View.Running.edit_window = new Bn.Admin.View.EditWindow();
            }
            var edit_window = Bn.Admin.View.Running.edit_window;
            edit_window.saveBtn.setText('修 改');
            edit_window.resetBtn.setVisible(true);
            edit_window.setTitle('修改系统管理人员');
            var sel = this.getSelectionModel().getSelected();
            edit_window.editForm.form.loadRecord(sel);
            edit_window.savetype = 1;

            edit_window.show();
            edit_window.maximize();
        },
        /**
		 * 删除系统管理人员
		 */
        deleteAdmin: function () {
            Ext.Msg.confirm('提示', '确实要删除所选的系统管理人员吗?', this.confirmDeleteAdmin, this);
        },
        /**
		 * 确认删除系统管理人员
		 */
        confirmDeleteAdmin: function (btn) {
            if (btn == 'yes') {
                var del_admin_ids = "";
                var selectedRows = this.getSelectionModel().getSelections();
                for (var flag = 0; flag < selectedRows.length; flag++) {
                    del_admin_ids = del_admin_ids + selectedRows[flag].data.ID + ",";
                }
                ExtServiceAdmin.deleteByIds(del_admin_ids);
                this.doSelectAdmin();
                Ext.Msg.alert("提示", "删除成功！");
            }
        }
    }),
    /**
	 * 核心内容区
	 */
    Panel: Ext.extend(Ext.form.FormPanel, {
        constructor: function (config) {
            Bn.Admin.View.Running.adminGrid = new Bn.Admin.View.Grid();
            if (Bn.Admin.Config.View.IsFix == 0) {
                Bn.Admin.View.Running.adminGrid.tvpView.menu.mBind.setChecked(false, true);
            }
            config = Ext.apply({
                region: 'center', layout: 'fit', frame: true,
                items: {
                    layout: 'border',
                    items: [
						Bn.Admin.View.Running.adminGrid,
						{ region: 'north', ref: 'north', layout: 'fit', collapseMode: 'mini', border: false, split: true, hidden: true },
						{ region: 'south', ref: 'south', layout: 'fit', collapseMode: 'mini', border: false, split: true, hidden: true, items: [Bn.Admin.View.Running.viewTabs] },
						{ region: 'west', ref: 'west', layout: 'fit', collapseMode: 'mini', border: false, split: true, hidden: true },
						{ region: 'east', ref: 'east', layout: 'fit', collapseMode: 'mini', border: false, split: true, hidden: true }
                    ]
                }
            }, config);
            Bn.Admin.View.Panel.superclass.constructor.call(this, config);
        }
    }),
    /**
	 * 当前运行的可视化对象
	 */
    Running: {
        /**
		 * 当前系统管理人员Grid对象
		 */
        adminGrid: null,

        /**
		 * 显示系统管理人员信息及关联信息列表的Tab页
		 */
        viewTabs: null,
        /**
		 * 当前创建的编辑窗口
		 */
        edit_window: null,
        /**
		 * 当前的显示窗口
		 */
        view_window: null
    }
};
/**
 * Controller:主程序
 */
Ext.onReady(function () {
    Ext.QuickTips.init();
    Ext.state.Manager.setProvider(Bn.Admin.Cookie);
    Ext.Direct.addProvider(Ext.app.remote_admin);
    Bn.Admin.Init();
    /**
	 * 系统管理人员数据模型获取数据Direct调用
	 */
    Bn.Admin.Store.adminStore.proxy = new Ext.data.DirectProxy({
        api: { read: ExtServiceAdmin.queryPageAdmin }
    });
    /**
	 * 系统管理人员页面布局
	 */
    Bn.Admin.Viewport = new Ext.Viewport({
        layout: 'border',
        items: [new Bn.Admin.View.Panel()]
    });
    Bn.Admin.Viewport.doLayout();
    setTimeout(function () {
        Ext.get('loading').remove();
        Ext.get('loading-mask').fadeOut({
            remove: true
        });
    }, 250);
});