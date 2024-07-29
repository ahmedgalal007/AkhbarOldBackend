Ext.data.JsonP.CKEDITOR_plugins_pastefromword_styles({"tagname":"class","name":"CKEDITOR.plugins.pastefromword.styles","alternateClassNames":[],"members":[{"name":"inliner","tagname":"property","owner":"CKEDITOR.plugins.pastefromword.styles","id":"property-inliner","meta":{"private":true}},{"name":"createStyleStack","tagname":"method","owner":"CKEDITOR.plugins.pastefromword.styles","id":"method-createStyleStack","meta":{"private":true}},{"name":"normalizedStyles","tagname":"method","owner":"CKEDITOR.plugins.pastefromword.styles","id":"method-normalizedStyles","meta":{"private":true}},{"name":"pushStylesLower","tagname":"method","owner":"CKEDITOR.plugins.pastefromword.styles","id":"method-pushStylesLower","meta":{}}],"aliases":{},"files":[{"filename":"","href":""}],"component":false,"superclasses":[],"subclasses":[],"mixedInto":[],"mixins":[],"parentMixins":[],"requires":[],"uses":[],"html":"<div><div class='doc-contents'>\n</div><div class='members'><div class='members-section'><div class='definedBy'>Defined By</div><h3 class='members-title icon-property'>Properties</h3><div class='subsection'><div id='property-inliner' class='member first-child not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.pastefromword.styles'>CKEDITOR.plugins.pastefromword.styles</span><br/><a href='source/default.html#CKEDITOR-plugins-pastefromword-styles-property-inliner' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.pastefromword.styles-property-inliner' class='name expandable'>inliner</a> : Object<span class=\"signature\"><span class='private' >private</span></span></div><div class='description'><div class='short'><p>Namespace containing the styles inliner.</p>\n</div><div class='long'><p>Namespace containing the styles inliner.</p>\n        <p>Available since: <b>4.7.0</b></p>\n</div></div></div></div></div><div class='members-section'><div class='definedBy'>Defined By</div><h3 class='members-title icon-method'>Methods</h3><div class='subsection'><div id='method-createStyleStack' class='member first-child not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.pastefromword.styles'>CKEDITOR.plugins.pastefromword.styles</span><br/><a href='source/default.html#CKEDITOR-plugins-pastefromword-styles-method-createStyleStack' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.pastefromword.styles-method-createStyleStack' class='name expandable'>createStyleStack</a>( <span class='pre'>element, filter, editor, [skipStyles]</span> )<span class=\"signature\"><span class='private' >private</span></span></div><div class='description'><div class='short'>Surrounds the element's children with a stack of spans, each one having one style\noriginally belonging to the element. ...</div><div class='long'><p>Surrounds the element's children with a stack of spans, each one having one style\noriginally belonging to the element.</p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>element</span> : <a href=\"#!/api/CKEDITOR.htmlParser.element\" rel=\"CKEDITOR.htmlParser.element\" class=\"docClass\">CKEDITOR.htmlParser.element</a><div class='sub-desc'>\n</div></li><li><span class='pre'>filter</span> : <a href=\"#!/api/CKEDITOR.htmlParser.filter\" rel=\"CKEDITOR.htmlParser.filter\" class=\"docClass\">CKEDITOR.htmlParser.filter</a><div class='sub-desc'>\n</div></li><li><span class='pre'>editor</span> : <a href=\"#!/api/CKEDITOR.editor\" rel=\"CKEDITOR.editor\" class=\"docClass\">CKEDITOR.editor</a><div class='sub-desc'>\n</div></li><li><span class='pre'>skipStyles</span> : RegExp (optional)<div class='sub-desc'><p>All matching style names will not be extracted to a style stack. Defaults\nto <code>/margin|text\\-align|width|border|padding/i</code>.</p>\n</div></li></ul></div></div></div><div id='method-normalizedStyles' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.pastefromword.styles'>CKEDITOR.plugins.pastefromword.styles</span><br/><a href='source/default.html#CKEDITOR-plugins-pastefromword-styles-method-normalizedStyles' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.pastefromword.styles-method-normalizedStyles' class='name expandable'>normalizedStyles</a>( <span class='pre'>element, editor</span> )<span class=\"signature\"><span class='private' >private</span></span></div><div class='description'><div class='short'>Filters Word-specific styles for a given element. ...</div><div class='long'><p>Filters Word-specific styles for a given element. Also might filter additional styles\nbased on the <code>editor</code> configuration.</p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>element</span> : <a href=\"#!/api/CKEDITOR.htmlParser.element\" rel=\"CKEDITOR.htmlParser.element\" class=\"docClass\">CKEDITOR.htmlParser.element</a><div class='sub-desc'>\n</div></li><li><span class='pre'>editor</span> : <a href=\"#!/api/CKEDITOR.editor\" rel=\"CKEDITOR.editor\" class=\"docClass\">CKEDITOR.editor</a><div class='sub-desc'>\n</div></li></ul></div></div></div><div id='method-pushStylesLower' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.plugins.pastefromword.styles'>CKEDITOR.plugins.pastefromword.styles</span><br/><a href='source/default.html#CKEDITOR-plugins-pastefromword-styles-method-pushStylesLower' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.plugins.pastefromword.styles-method-pushStylesLower' class='name expandable'>pushStylesLower</a>( <span class='pre'>element, exceptions, [wrapText]</span> ) : Boolean<span class=\"signature\"></span></div><div class='description'><div class='short'>Moves the element's styles lower in the DOM hierarchy. ...</div><div class='long'><p>Moves the element's styles lower in the DOM hierarchy. If <code>wrapText==true</code> and the direct child of an element\nis a text node it will be wrapped in a <code>span</code> element.</p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>element</span> : <a href=\"#!/api/CKEDITOR.htmlParser.element\" rel=\"CKEDITOR.htmlParser.element\" class=\"docClass\">CKEDITOR.htmlParser.element</a><div class='sub-desc'>\n</div></li><li><span class='pre'>exceptions</span> : Object<div class='sub-desc'><p>An object containing style names which should not be moved, e.g. <code>{ background: true }</code>.</p>\n</div></li><li><span class='pre'>wrapText</span> : Boolean (optional)<div class='sub-desc'><p>Whether a direct text child of an element should be wrapped into a <code>span</code> tag\nso that the styles can be moved to it.</p>\n<p>Defaults to: <code>false</code></p></div></li></ul><h3 class='pa'>Returns</h3><ul><li><span class='pre'>Boolean</span><div class='sub-desc'><p>Returns <code>true</code> if styles were successfully moved lower.</p>\n</div></li></ul></div></div></div></div></div></div></div>","meta":{}});