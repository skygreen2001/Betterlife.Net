Ext.namespace("BetterlifeNet.Admin");
Bn = BetterlifeNet.Admin;
/**
 * 导航功能
 */
Bn.Navigation = {
	/**
	 * 在指定的TabPanel组件上添加Tab
	 * 【如果已经添加了则激活该Tab】
	 * @param contentPanel TabPanel component组件
	 * @param title Tab头标题
	 * @param html Tab页面内的内容
	 * @param id 添加的Tab的标识
	 */
    AddTab: function (contentPanel, title, html, id) {
		var cpId = 'cp-' + id;
		var tab = Ext.getCmp(cpId);
		if (tab){
			contentPanel.setActiveTab(tab);
		}else{
			contentPanel.add({
				title: title,
				html: html,
				closable:true
			}).show();
		}
	},
	/**
	 * 页面导航在Tab内嵌一个Ifame组件
	 */
	IFrameComponent : Ext.extend(Ext.BoxComponent, {
		onRender : function(ct, position){
			this.el = ct.createChild({tag: 'iframe', id: this.id,frameBorder:0, src: this.url});
		}
	}),
	/**
	 * 根据指定的Url在指定的TabPanel组件上添加Tab
	 * 【如果已经添加了则激活该Tab】
	 * @param contentPanel TabPanel component组件
	 * @param title Tab头标题
	 * @param url 指定的Url
	 * @param id 添加的Tab的标识
	 */
	AddTabbyUrl: function (contentPanel, title, url, id) {
		var cpId = 'cp-' + id;
		var tab =Ext.getCmp(cpId);
		var isComponent=true;
		if (url){
			if(url.indexOf("go=")>0)url=url+"&".concat(Math.random());
		}
		if (!Bn.Config.IsTabHeaderShow) {
		    var centerArea = Ext.get("centerArea");
		    centerArea.setHeight(contentPanel.getHeight());
		    centerArea.setWidth(contentPanel.getWidth());
		    centerArea
					.update("<iframe id='frm" + id + "' name='frm" + id + "' scrolling='auto' width='100%' height='100%'  frameborder='0' src='"
							+ url + "'></iframe>");
		    contentPanel.setTitle(title, "tabs");
		    return;
		}
		if (tab){
			contentPanel.setActiveTab(tab);
		}else{
			if (isComponent){
				tab = new Ext.Panel({
					 id: cpId,
					 title: title,
					 tabTip:title,
					 url:url,
					 closable:true,
					 // layout to fit child component
					 layout:'fit',
					 border:false,
					 // add iframe as the child component
					 items: [ new Bn.Navigation.IFrameComponent({ id: "frm"+id, url: url })]
				});
				contentPanel.add(tab).show();
			}else{
				contentPanel.add({
					id: cpId,
					tabTip:title,
					url:url,
					title: title,
					html:"<iframe id='frm"+id+"' name='frm"+id+"' scrolling='auto' width='100%' height='100%'  frameborder='0' src='"+url+"'> </iframe>",
					closable:true
				}).show();
			}
		}
	},
	/**
	 * 改写超链接默认事件，使新打开的页面都显示在指定的TabPanel组件上。
	 */
	HyperlinkClicked:function(e){
		e.preventDefault();
		var linkTarget=e.target;
		var title="";
		if (Ext.isIE){
			title=linkTarget.innerText;
		}else{
			title=linkTarget.text;
		}
		if (linkTarget.id == "logout") {
		    window.location.href = "/Index/Logout";
		} else {
		    Bn.Navigation.AddTabbyUrl(Bn.Viewport.center, title, linkTarget.href, linkTarget.id);
		}
	},
	OnContextMenu:function(e, item){
		if (item.href){
			if (Ext.isGecko){
				e.preventDefault();
			}
			var m = new Ext.menu.Menu({
				items: [{
					itemId: 'openInNewIETab',
					text: '在新标签页中打开',
					scope: this,
					handler: function(){
						window.open(item.href,"_blank");
					}
				}]
			});
			m.setWidth(150);
			e.stopEvent();
			m.showAt(e.getPoint());
		}
	}
};