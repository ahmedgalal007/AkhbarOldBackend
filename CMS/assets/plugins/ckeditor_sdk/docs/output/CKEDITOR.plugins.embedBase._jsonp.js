Ext.data.JsonP.CKEDITOR_plugins_embedBase__jsonp({"tagname":"class","name":"CKEDITOR.plugins.embedBase._jsonp","autodetected":{},"files":[{"filename":"plugin.js","href":"plugin33.html#CKEDITOR-plugins-embedBase-_jsonp"}],"private":true,"singleton":true,"members":[{"name":"_attachScript","tagname":"method","owner":"CKEDITOR.plugins.embedBase._jsonp","id":"method-_attachScript","meta":{"private":true}},{"name":"sendRequest","tagname":"method","owner":"CKEDITOR.plugins.embedBase._jsonp","id":"method-sendRequest","meta":{}}],"alternateClassNames":[],"aliases":{},"id":"class-CKEDITOR.plugins.embedBase._jsonp","component":false,"superclasses":[],"subclasses":[],"mixedInto":[],"mixins":[],"parentMixins":[],"requires":[],"uses":[],"html":"<div><pre class=\"hierarchy\"><h4>Files</h4><div class='dependency'><a href='source/plugin33.html#CKEDITOR-plugins-embedBase-_jsonp' target='_blank'>plugin.js</a></div></pre><div class='doc-contents'><div class='rounded-box private-box'><p><strong>NOTE:</strong> This is a private utility class for internal use by the framework. Don't rely on its existence.</p></div><p>JSONP communication.</p>\n</div><div class='members'><div class='members-section'><div class='definedBy'>Defined By</div><h3 class='members-title icon-method'>Methods</h3><div class='subsection'><div id='method-_attachScript' class='member first-child not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.embedBase._jsonp'>CKEDITOR.plugins.embedBase._jsonp</span><br/><a href='source/plugin33.html#CKEDITOR-plugins-embedBase-_jsonp-method-_attachScript' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.embedBase._jsonp-method-_attachScript' class='name expandable'>_attachScript</a>( <span class='pre'>url, errorCallback</span> )<span class=\"signature\"><span class='private' >private</span></span></div><div class='description'><div class='short'>Creates a &lt;script&gt; element and attaches it to the document &lt;body&gt;. ...</div><div class='long'><p>Creates a <code>&lt;script&gt;</code> element and attaches it to the document <code>&lt;body&gt;</code>.</p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>url</span> : Object<div class='sub-desc'></div></li><li><span class='pre'>errorCallback</span> : Object<div class='sub-desc'></div></li></ul></div></div></div><div id='method-sendRequest' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.embedBase._jsonp'>CKEDITOR.plugins.embedBase._jsonp</span><br/><a href='source/plugin33.html#CKEDITOR-plugins-embedBase-_jsonp-method-sendRequest' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.embedBase._jsonp-method-sendRequest' class='name expandable'>sendRequest</a>( <span class='pre'>urlTemplate, urlParams, callback, [errorCallback]</span> ) : Object<span class=\"signature\"></span></div><div class='description'><div class='short'>Sends a request using the JSONP technique. ...</div><div class='long'><p>Sends a request using the JSONP technique.</p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>urlTemplate</span> : <a href=\"#!/api/CKEDITOR.template\" rel=\"CKEDITOR.template\" class=\"docClass\">CKEDITOR.template</a><div class='sub-desc'><p>The template of the URL to be requested. All properties\npassed in <code>urlParams</code> can be used, plus a <code>{callback}</code>, which represents a JSONP callback, must be defined.</p>\n</div></li><li><span class='pre'>urlParams</span> : Object<div class='sub-desc'><p>Parameters to be passed to the <code>urlTemplate</code>.</p>\n</div></li><li><span class='pre'>callback</span> : Function<div class='sub-desc'>\n</div></li><li><span class='pre'>errorCallback</span> : Function (optional)<div class='sub-desc'>\n</div></li></ul><h3 class='pa'>Returns</h3><ul><li><span class='pre'>Object</span><div class='sub-desc'><p>The request object with a <code>cancel()</code> method.</p>\n</div></li></ul></div></div></div></div></div></div></div>","meta":{"private":true}});