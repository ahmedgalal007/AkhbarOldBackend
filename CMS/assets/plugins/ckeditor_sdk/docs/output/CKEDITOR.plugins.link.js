Ext.data.JsonP.CKEDITOR_plugins_link({"tagname":"class","name":"CKEDITOR.plugins.link","autodetected":{},"files":[{"filename":"plugin.js","href":"plugin31.html#CKEDITOR-plugins-link"}],"singleton":true,"members":[{"name":"emptyAnchorFix","tagname":"property","owner":"CKEDITOR.plugins.link","id":"property-emptyAnchorFix","meta":{"deprecated":{"text":"<p>It is set to <code>false</code> in every browser.</p>\n","version":"4.3.3"},"readonly":true}},{"name":"fakeAnchor","tagname":"property","owner":"CKEDITOR.plugins.link","id":"property-fakeAnchor","meta":{"deprecated":{"text":"<p>It is set to <code>true</code> in every browser.</p>\n","version":"4.3.3"},"readonly":true}},{"name":"synAnchorSelector","tagname":"property","owner":"CKEDITOR.plugins.link","id":"property-synAnchorSelector","meta":{"deprecated":{"text":"<p>It is set to <code>false</code> in every browser.</p>\n","version":"4.3.3"},"readonly":true}},{"name":"getEditorAnchors","tagname":"method","owner":"CKEDITOR.plugins.link","id":"method-getEditorAnchors","meta":{}},{"name":"getLinkAttributes","tagname":"method","owner":"CKEDITOR.plugins.link","id":"method-getLinkAttributes","meta":{}},{"name":"getSelectedLink","tagname":"method","owner":"CKEDITOR.plugins.link","id":"method-getSelectedLink","meta":{}},{"name":"parseLinkAttributes","tagname":"method","owner":"CKEDITOR.plugins.link","id":"method-parseLinkAttributes","meta":{}},{"name":"showDisplayTextForElement","tagname":"method","owner":"CKEDITOR.plugins.link","id":"method-showDisplayTextForElement","meta":{}},{"name":"tryRestoreFakeAnchor","tagname":"method","owner":"CKEDITOR.plugins.link","id":"method-tryRestoreFakeAnchor","meta":{}}],"alternateClassNames":[],"aliases":{},"id":"class-CKEDITOR.plugins.link","component":false,"superclasses":[],"subclasses":[],"mixedInto":[],"mixins":[],"parentMixins":[],"requires":[],"uses":[],"html":"<div><pre class=\"hierarchy\"><h4>Files</h4><div class='dependency'><a href='source/plugin31.html#CKEDITOR-plugins-link' target='_blank'>plugin.js</a></div></pre><div class='doc-contents'><p>Set of Link plugin helpers.</p>\n</div><div class='members'><div class='members-section'><div class='definedBy'>Defined By</div><h3 class='members-title icon-property'>Properties</h3><div class='subsection'><div id='property-emptyAnchorFix' class='member first-child not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.link'>CKEDITOR.plugins.link</span><br/><a href='source/plugin31.html#CKEDITOR-plugins-link-property-emptyAnchorFix' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.link-property-emptyAnchorFix' class='name expandable'>emptyAnchorFix</a> : Boolean<span class=\"signature\"><span class='deprecated' >deprecated</span><span class='readonly' >readonly</span></span></div><div class='description'><div class='short'>For browsers that have editing issues with an empty anchor. ...</div><div class='long'><p>For browsers that have editing issues with an empty anchor.</p>\n        <div class='rounded-box deprecated-box deprecated-tag-box'>\n        <p>This property has been <strong>deprected</strong> since 4.3.3</p>\n        <p>It is set to <code>false</code> in every browser.</p>\n\n        </div>\n</div></div></div><div id='property-fakeAnchor' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.link'>CKEDITOR.plugins.link</span><br/><a href='source/plugin31.html#CKEDITOR-plugins-link-property-fakeAnchor' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.link-property-fakeAnchor' class='name expandable'>fakeAnchor</a> : Boolean<span class=\"signature\"><span class='deprecated' >deprecated</span><span class='readonly' >readonly</span></span></div><div class='description'><div class='short'>Opera and WebKit do not make it possible to select empty anchors. ...</div><div class='long'><p>Opera and WebKit do not make it possible to select empty anchors. Fake\nelements must be used for them.</p>\n<p>Defaults to: <code>true</code></p>        <div class='rounded-box deprecated-box deprecated-tag-box'>\n        <p>This property has been <strong>deprected</strong> since 4.3.3</p>\n        <p>It is set to <code>true</code> in every browser.</p>\n\n        </div>\n</div></div></div><div id='property-synAnchorSelector' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.link'>CKEDITOR.plugins.link</span><br/><a href='source/plugin31.html#CKEDITOR-plugins-link-property-synAnchorSelector' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.link-property-synAnchorSelector' class='name expandable'>synAnchorSelector</a> : Boolean<span class=\"signature\"><span class='deprecated' >deprecated</span><span class='readonly' >readonly</span></span></div><div class='description'><div class='short'>For browsers that do not support CSS3 a[name]:empty(). ...</div><div class='long'><p>For browsers that do not support CSS3 <code>a[name]:empty()</code>. Note that IE9 is included because of #7783.</p>\n        <div class='rounded-box deprecated-box deprecated-tag-box'>\n        <p>This property has been <strong>deprected</strong> since 4.3.3</p>\n        <p>It is set to <code>false</code> in every browser.</p>\n\n        </div>\n</div></div></div></div></div><div class='members-section'><div class='definedBy'>Defined By</div><h3 class='members-title icon-method'>Methods</h3><div class='subsection'><div id='method-getEditorAnchors' class='member first-child not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.link'>CKEDITOR.plugins.link</span><br/><a href='source/plugin31.html#CKEDITOR-plugins-link-method-getEditorAnchors' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.link-method-getEditorAnchors' class='name expandable'>getEditorAnchors</a>( <span class='pre'>editor</span> ) : <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a>[]<span class=\"signature\"></span></div><div class='description'><div class='short'>Collects anchors available in the editor (i.e. ...</div><div class='long'><p>Collects anchors available in the editor (i.e. used by the Link plugin).\nNote that the scope of search is different for inline (the \"global\" document) and\nclassic (<code>iframe</code>-based) editors (the \"inner\" document).</p>\n        <p>Available since: <b>4.3.3</b></p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>editor</span> : <a href=\"#!/api/CKEDITOR.editor\" rel=\"CKEDITOR.editor\" class=\"docClass\">CKEDITOR.editor</a><div class='sub-desc'>\n</div></li></ul><h3 class='pa'>Returns</h3><ul><li><span class='pre'><a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a>[]</span><div class='sub-desc'><p>An array of anchor elements.</p>\n</div></li></ul></div></div></div><div id='method-getLinkAttributes' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.link'>CKEDITOR.plugins.link</span><br/><a href='source/plugin31.html#CKEDITOR-plugins-link-method-getLinkAttributes' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.link-method-getLinkAttributes' class='name expandable'>getLinkAttributes</a>( <span class='pre'>editor, data</span> ) : Object<span class=\"signature\"></span></div><div class='description'><div class='short'>Converts link data produced by parseLinkAttributes into an object which consists\nof attributes to be set (with their ...</div><div class='long'><p>Converts link data produced by <a href=\"#!/api/CKEDITOR.plugins.link-method-parseLinkAttributes\" rel=\"CKEDITOR.plugins.link-method-parseLinkAttributes\" class=\"docClass\">parseLinkAttributes</a> into an object which consists\nof attributes to be set (with their values) and an array of attributes to be removed.\nThis method can be used to compose or to update any link element with the given data.</p>\n        <p>Available since: <b>4.4</b></p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>editor</span> : <a href=\"#!/api/CKEDITOR.editor\" rel=\"CKEDITOR.editor\" class=\"docClass\">CKEDITOR.editor</a><div class='sub-desc'>\n</div></li><li><span class='pre'>data</span> : Object<div class='sub-desc'><p>Data in <a href=\"#!/api/CKEDITOR.plugins.link-method-parseLinkAttributes\" rel=\"CKEDITOR.plugins.link-method-parseLinkAttributes\" class=\"docClass\">parseLinkAttributes</a> format.</p>\n</div></li></ul><h3 class='pa'>Returns</h3><ul><li><span class='pre'>Object</span><div class='sub-desc'><p>An object consisting of two keys, i.e.:</p>\n\n<pre><code>{\n    // Attributes to be set.\n    set: {\n        href: 'http://foo.bar',\n        target: 'bang'\n    },\n    // Attributes to be removed.\n    removed: [\n        'id', 'style'\n    ]\n}\n</code></pre>\n</div></li></ul></div></div></div><div id='method-getSelectedLink' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.link'>CKEDITOR.plugins.link</span><br/><a href='source/plugin31.html#CKEDITOR-plugins-link-method-getSelectedLink' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.link-method-getSelectedLink' class='name expandable'>getSelectedLink</a>( <span class='pre'>editor, [returnMultiple]</span> ) : <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a>/<a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a>[]/null<span class=\"signature\"></span></div><div class='description'><div class='short'>Get the surrounding link element of the current selection. ...</div><div class='long'><p>Get the surrounding link element of the current selection.</p>\n\n<pre><code><a href=\"#!/api/CKEDITOR.plugins.link-method-getSelectedLink\" rel=\"CKEDITOR.plugins.link-method-getSelectedLink\" class=\"docClass\">CKEDITOR.plugins.link.getSelectedLink</a>( editor );\n\n// The following selections will all return the link element.\n\n&lt;a href=\"#\"&gt;li^nk&lt;/a&gt;\n&lt;a href=\"#\"&gt;[link]&lt;/a&gt;\ntext[&lt;a href=\"#\"&gt;link]&lt;/a&gt;\n&lt;a href=\"#\"&gt;li[nk&lt;/a&gt;]\n[&lt;b&gt;&lt;a href=\"#\"&gt;li]nk&lt;/a&gt;&lt;/b&gt;]\n[&lt;a href=\"#\"&gt;&lt;b&gt;li]nk&lt;/b&gt;&lt;/a&gt;\n</code></pre>\n        <p>Available since: <b>3.2.1</b></p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>editor</span> : <a href=\"#!/api/CKEDITOR.editor\" rel=\"CKEDITOR.editor\" class=\"docClass\">CKEDITOR.editor</a><div class='sub-desc'>\n</div></li><li><span class='pre'>returnMultiple</span> : Boolean (optional)<div class='sub-desc'><p>Indicates whether the function should return only the first selected link or all of them.</p>\n<p>Defaults to: <code>false</code></p></div></li></ul><h3 class='pa'>Returns</h3><ul><li><span class='pre'><a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a>/<a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a>[]/null</span><div class='sub-desc'><p>A single link element or an array of link\nelements relevant to the current selection.</p>\n</div></li></ul></div></div></div><div id='method-parseLinkAttributes' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.link'>CKEDITOR.plugins.link</span><br/><a href='source/plugin31.html#CKEDITOR-plugins-link-method-parseLinkAttributes' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.link-method-parseLinkAttributes' class='name expandable'>parseLinkAttributes</a>( <span class='pre'>editor, element</span> ) : Object<span class=\"signature\"></span></div><div class='description'><div class='short'>Parses attributes of the link element and returns an object representing\nthe current state (data) of the link. ...</div><div class='long'><p>Parses attributes of the link element and returns an object representing\nthe current state (data) of the link. This data format is a plain object accepted\ne.g. by the Link dialog window and <a href=\"#!/api/CKEDITOR.plugins.link-method-getLinkAttributes\" rel=\"CKEDITOR.plugins.link-method-getLinkAttributes\" class=\"docClass\">getLinkAttributes</a>.</p>\n\n<p><strong>Note:</strong> Data model format produced by the parser must be compatible with the Link\nplugin dialog because it is passed directly to <a href=\"#!/api/CKEDITOR.dialog-method-setupContent\" rel=\"CKEDITOR.dialog-method-setupContent\" class=\"docClass\">CKEDITOR.dialog.setupContent</a>.</p>\n        <p>Available since: <b>4.4</b></p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>editor</span> : <a href=\"#!/api/CKEDITOR.editor\" rel=\"CKEDITOR.editor\" class=\"docClass\">CKEDITOR.editor</a><div class='sub-desc'>\n</div></li><li><span class='pre'>element</span> : <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a><div class='sub-desc'>\n</div></li></ul><h3 class='pa'>Returns</h3><ul><li><span class='pre'>Object</span><div class='sub-desc'><p>An object of link data.</p>\n</div></li></ul></div></div></div><div id='method-showDisplayTextForElement' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.link'>CKEDITOR.plugins.link</span><br/><a href='source/plugin31.html#CKEDITOR-plugins-link-method-showDisplayTextForElement' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.link-method-showDisplayTextForElement' class='name expandable'>showDisplayTextForElement</a>( <span class='pre'>element, editor</span> ) : Boolean<span class=\"signature\"></span></div><div class='description'><div class='short'>Determines whether an element should have a \"Display Text\" field in the Link dialog. ...</div><div class='long'><p>Determines whether an element should have a \"Display Text\" field in the Link dialog.</p>\n        <p>Available since: <b>4.5.11</b></p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>element</span> : <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a>/null<div class='sub-desc'><p>Selected element, <code>null</code> if none selected or if a ranged selection\nis made.</p>\n</div></li><li><span class='pre'>editor</span> : <a href=\"#!/api/CKEDITOR.editor\" rel=\"CKEDITOR.editor\" class=\"docClass\">CKEDITOR.editor</a><div class='sub-desc'><p>The editor instance for which the check is performed.</p>\n</div></li></ul><h3 class='pa'>Returns</h3><ul><li><span class='pre'>Boolean</span><div class='sub-desc'>\n</div></li></ul></div></div></div><div id='method-tryRestoreFakeAnchor' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.link'>CKEDITOR.plugins.link</span><br/><a href='source/plugin31.html#CKEDITOR-plugins-link-method-tryRestoreFakeAnchor' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.link-method-tryRestoreFakeAnchor' class='name expandable'>tryRestoreFakeAnchor</a>( <span class='pre'>editor, element</span> ) : <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a><span class=\"signature\"></span></div><div class='description'><div class='short'>Returns an element representing a real anchor restored from a fake anchor. ...</div><div class='long'><p>Returns an element representing a real anchor restored from a fake anchor.</p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>editor</span> : <a href=\"#!/api/CKEDITOR.editor\" rel=\"CKEDITOR.editor\" class=\"docClass\">CKEDITOR.editor</a><div class='sub-desc'>\n</div></li><li><span class='pre'>element</span> : <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a><div class='sub-desc'>\n</div></li></ul><h3 class='pa'>Returns</h3><ul><li><span class='pre'><a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a></span><div class='sub-desc'><p>Restored anchor element or nothing if the\npassed element was not a fake anchor.</p>\n</div></li></ul></div></div></div></div></div></div></div>","meta":{}});