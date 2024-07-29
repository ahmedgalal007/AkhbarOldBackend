Ext.data.JsonP.CKEDITOR_htmlWriter({"tagname":"class","name":"CKEDITOR.htmlWriter","autodetected":{},"files":[{"filename":"plugin.js","href":"plugin54.html#CKEDITOR-htmlWriter"}],"extends":"CKEDITOR.htmlParser.basicWriter","members":[{"name":"indentationChars","tagname":"property","owner":"CKEDITOR.htmlWriter","id":"property-indentationChars","meta":{}},{"name":"lineBreakChars","tagname":"property","owner":"CKEDITOR.htmlWriter","id":"property-lineBreakChars","meta":{}},{"name":"selfClosingEnd","tagname":"property","owner":"CKEDITOR.htmlWriter","id":"property-selfClosingEnd","meta":{}},{"name":"constructor","tagname":"method","owner":"CKEDITOR.htmlWriter","id":"method-constructor","meta":{}},{"name":"attribute","tagname":"method","owner":"CKEDITOR.htmlWriter","id":"method-attribute","meta":{}},{"name":"closeTag","tagname":"method","owner":"CKEDITOR.htmlWriter","id":"method-closeTag","meta":{}},{"name":"comment","tagname":"method","owner":"CKEDITOR.htmlWriter","id":"method-comment","meta":{}},{"name":"getHtml","tagname":"method","owner":"CKEDITOR.htmlParser.basicWriter","id":"method-getHtml","meta":{}},{"name":"indentation","tagname":"method","owner":"CKEDITOR.htmlWriter","id":"method-indentation","meta":{}},{"name":"lineBreak","tagname":"method","owner":"CKEDITOR.htmlWriter","id":"method-lineBreak","meta":{}},{"name":"openTag","tagname":"method","owner":"CKEDITOR.htmlWriter","id":"method-openTag","meta":{}},{"name":"openTagClose","tagname":"method","owner":"CKEDITOR.htmlWriter","id":"method-openTagClose","meta":{}},{"name":"reset","tagname":"method","owner":"CKEDITOR.htmlWriter","id":"method-reset","meta":{}},{"name":"setRules","tagname":"method","owner":"CKEDITOR.htmlWriter","id":"method-setRules","meta":{}},{"name":"text","tagname":"method","owner":"CKEDITOR.htmlWriter","id":"method-text","meta":{}},{"name":"write","tagname":"method","owner":"CKEDITOR.htmlParser.basicWriter","id":"method-write","meta":{}}],"alternateClassNames":[],"aliases":{},"id":"class-CKEDITOR.htmlWriter","short_doc":"The class used to write HTML data. ...","component":false,"superclasses":["CKEDITOR.htmlParser.basicWriter"],"subclasses":[],"mixedInto":[],"mixins":[],"parentMixins":[],"requires":[],"uses":[],"html":"<div><pre class=\"hierarchy\"><h4>Hierarchy</h4><div class='subclass first-child'><a href='#!/api/CKEDITOR.htmlParser.basicWriter' rel='CKEDITOR.htmlParser.basicWriter' class='docClass'>CKEDITOR.htmlParser.basicWriter</a><div class='subclass '><strong>CKEDITOR.htmlWriter</strong></div></div><h4>Files</h4><div class='dependency'><a href='source/plugin54.html#CKEDITOR-htmlWriter' target='_blank'>plugin.js</a></div></pre><div class='doc-contents'><p>The class used to write HTML data.</p>\n\n<pre><code>var writer = new <a href=\"#!/api/CKEDITOR.htmlWriter\" rel=\"CKEDITOR.htmlWriter\" class=\"docClass\">CKEDITOR.htmlWriter</a>();\nwriter.openTag( 'p' );\nwriter.attribute( 'class', 'MyClass' );\nwriter.openTagClose( 'p' );\nwriter.text( 'Hello' );\nwriter.closeTag( 'p' );\nalert( writer.getHtml() ); // '&lt;p class=\"MyClass\"&gt;Hello&lt;/p&gt;'\n</code></pre>\n</div><div class='members'><div class='members-section'><div class='definedBy'>Defined By</div><h3 class='members-title icon-property'>Properties</h3><div class='subsection'><div id='property-indentationChars' class='member first-child not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.htmlWriter'>CKEDITOR.htmlWriter</span><br/><a href='source/plugin54.html#CKEDITOR-htmlWriter-property-indentationChars' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlWriter-property-indentationChars' class='name expandable'>indentationChars</a> : String<span class=\"signature\"></span></div><div class='description'><div class='short'>The characters to be used for each indentation step. ...</div><div class='long'><p>The characters to be used for each indentation step.</p>\n\n<pre><code>// Use tab for indentation.\neditorInstance.dataProcessor.writer.indentationChars = '\\t';\n</code></pre>\n<p>Defaults to: <code>&#39;\\t&#39;</code></p></div></div></div><div id='property-lineBreakChars' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.htmlWriter'>CKEDITOR.htmlWriter</span><br/><a href='source/plugin54.html#CKEDITOR-htmlWriter-property-lineBreakChars' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlWriter-property-lineBreakChars' class='name expandable'>lineBreakChars</a> : String<span class=\"signature\"></span></div><div class='description'><div class='short'>The characters to be used for line breaks. ...</div><div class='long'><p>The characters to be used for line breaks.</p>\n\n<pre><code>// Use CRLF for line breaks.\neditorInstance.dataProcessor.writer.lineBreakChars = '\\r\\n';\n</code></pre>\n<p>Defaults to: <code>&#39;\\n&#39;</code></p></div></div></div><div id='property-selfClosingEnd' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.htmlWriter'>CKEDITOR.htmlWriter</span><br/><a href='source/plugin54.html#CKEDITOR-htmlWriter-property-selfClosingEnd' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlWriter-property-selfClosingEnd' class='name expandable'>selfClosingEnd</a> : String<span class=\"signature\"></span></div><div class='description'><div class='short'>The characters to be used to close \"self-closing\" elements, like &lt;br&gt; or &lt;img&gt;. ...</div><div class='long'><p>The characters to be used to close \"self-closing\" elements, like <code>&lt;br&gt;</code> or <code>&lt;img&gt;</code>.</p>\n\n<pre><code>// Use HTML4 notation for self-closing elements.\neditorInstance.dataProcessor.writer.selfClosingEnd = '&gt;';\n</code></pre>\n<p>Defaults to: <code>&#39; /&gt;&#39;</code></p></div></div></div></div></div><div class='members-section'><div class='definedBy'>Defined By</div><h3 class='members-title icon-method'>Methods</h3><div class='subsection'><div id='method-constructor' class='member first-child not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.htmlWriter'>CKEDITOR.htmlWriter</span><br/><a href='source/plugin54.html#CKEDITOR-htmlWriter-method-constructor' target='_blank' class='view-source'>view source</a></div><strong class='new-keyword'>new</strong><a href='#!/api/CKEDITOR.htmlWriter-method-constructor' class='name expandable'>CKEDITOR.htmlWriter</a>( <span class='pre'></span> ) : <a href=\"#!/api/CKEDITOR.htmlWriter\" rel=\"CKEDITOR.htmlWriter\" class=\"docClass\">CKEDITOR.htmlWriter</a><span class=\"signature\"></span></div><div class='description'><div class='short'>Creates an htmlWriter class instance. ...</div><div class='long'><p>Creates an <code>htmlWriter</code> class instance.</p>\n<h3 class='pa'>Returns</h3><ul><li><span class='pre'><a href=\"#!/api/CKEDITOR.htmlWriter\" rel=\"CKEDITOR.htmlWriter\" class=\"docClass\">CKEDITOR.htmlWriter</a></span><div class='sub-desc'>\n</div></li></ul><p>Overrides: <a href=\"#!/api/CKEDITOR.htmlParser.basicWriter-method-constructor\" rel=\"CKEDITOR.htmlParser.basicWriter-method-constructor\" class=\"docClass\">CKEDITOR.htmlParser.basicWriter.constructor</a></p></div></div></div><div id='method-attribute' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.htmlWriter'>CKEDITOR.htmlWriter</span><br/><a href='source/plugin54.html#CKEDITOR-htmlWriter-method-attribute' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlWriter-method-attribute' class='name expandable'>attribute</a>( <span class='pre'>attName, attValue</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Writes an attribute. ...</div><div class='long'><p>Writes an attribute. This function should be called after opening the\ntag with <a href=\"#!/api/CKEDITOR.htmlWriter-method-openTagClose\" rel=\"CKEDITOR.htmlWriter-method-openTagClose\" class=\"docClass\">openTagClose</a>.</p>\n\n<pre><code>// Writes ' class=\"MyClass\"'.\nwriter.attribute( 'class', 'MyClass' );\n</code></pre>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>attName</span> : String<div class='sub-desc'><p>The attribute name.</p>\n</div></li><li><span class='pre'>attValue</span> : String<div class='sub-desc'><p>The attribute value.</p>\n</div></li></ul><p>Overrides: <a href=\"#!/api/CKEDITOR.htmlParser.basicWriter-method-attribute\" rel=\"CKEDITOR.htmlParser.basicWriter-method-attribute\" class=\"docClass\">CKEDITOR.htmlParser.basicWriter.attribute</a></p></div></div></div><div id='method-closeTag' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.htmlWriter'>CKEDITOR.htmlWriter</span><br/><a href='source/plugin54.html#CKEDITOR-htmlWriter-method-closeTag' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlWriter-method-closeTag' class='name expandable'>closeTag</a>( <span class='pre'>tagName</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Writes a closer tag. ...</div><div class='long'><p>Writes a closer tag.</p>\n\n<pre><code>// Writes '&lt;/p&gt;'.\nwriter.closeTag( 'p' );\n</code></pre>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>tagName</span> : String<div class='sub-desc'><p>The element name for this tag.</p>\n</div></li></ul><p>Overrides: <a href=\"#!/api/CKEDITOR.htmlParser.basicWriter-method-closeTag\" rel=\"CKEDITOR.htmlParser.basicWriter-method-closeTag\" class=\"docClass\">CKEDITOR.htmlParser.basicWriter.closeTag</a></p></div></div></div><div id='method-comment' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.htmlWriter'>CKEDITOR.htmlWriter</span><br/><a href='source/plugin54.html#CKEDITOR-htmlWriter-method-comment' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlWriter-method-comment' class='name expandable'>comment</a>( <span class='pre'>comment</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Writes a comment. ...</div><div class='long'><p>Writes a comment.</p>\n\n<pre><code>// Writes \"&lt;!-- My comment --&gt;\".\nwriter.comment( ' My comment ' );\n</code></pre>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>comment</span> : String<div class='sub-desc'><p>The comment text.</p>\n</div></li></ul><p>Overrides: <a href=\"#!/api/CKEDITOR.htmlParser.basicWriter-method-comment\" rel=\"CKEDITOR.htmlParser.basicWriter-method-comment\" class=\"docClass\">CKEDITOR.htmlParser.basicWriter.comment</a></p></div></div></div><div id='method-getHtml' class='member  inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><a href='#!/api/CKEDITOR.htmlParser.basicWriter' rel='CKEDITOR.htmlParser.basicWriter' class='defined-in docClass'>CKEDITOR.htmlParser.basicWriter</a><br/><a href='source/basicwriter.html#CKEDITOR-htmlParser-basicWriter-method-getHtml' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlParser.basicWriter-method-getHtml' class='name expandable'>getHtml</a>( <span class='pre'>reset</span> ) : String<span class=\"signature\"></span></div><div class='description'><div class='short'>Empties the current output buffer. ...</div><div class='long'><p>Empties the current output buffer.</p>\n\n<pre><code>var html = writer.getHtml();\n</code></pre>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>reset</span> : Boolean<div class='sub-desc'><p>Indicates that the <a href=\"#!/api/CKEDITOR.htmlParser.basicWriter-method-reset\" rel=\"CKEDITOR.htmlParser.basicWriter-method-reset\" class=\"docClass\">reset</a> method is to\nbe automatically called after retrieving the HTML.</p>\n</div></li></ul><h3 class='pa'>Returns</h3><ul><li><span class='pre'>String</span><div class='sub-desc'><p>The HTML written to the writer so far.</p>\n</div></li></ul></div></div></div><div id='method-indentation' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.htmlWriter'>CKEDITOR.htmlWriter</span><br/><a href='source/plugin54.html#CKEDITOR-htmlWriter-method-indentation' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlWriter-method-indentation' class='name expandable'>indentation</a>( <span class='pre'></span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Writes the current indentation character. ...</div><div class='long'><p>Writes the current indentation character. It uses the <a href=\"#!/api/CKEDITOR.htmlWriter-property-indentationChars\" rel=\"CKEDITOR.htmlWriter-property-indentationChars\" class=\"docClass\">indentationChars</a>\nproperty, repeating it for the current indentation steps.</p>\n\n<pre><code>// Writes '\\t' (e.g.).\nwriter.indentation();\n</code></pre>\n</div></div></div><div id='method-lineBreak' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.htmlWriter'>CKEDITOR.htmlWriter</span><br/><a href='source/plugin54.html#CKEDITOR-htmlWriter-method-lineBreak' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlWriter-method-lineBreak' class='name expandable'>lineBreak</a>( <span class='pre'></span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Writes a line break. ...</div><div class='long'><p>Writes a line break. It uses the <a href=\"#!/api/CKEDITOR.htmlWriter-property-lineBreakChars\" rel=\"CKEDITOR.htmlWriter-property-lineBreakChars\" class=\"docClass\">lineBreakChars</a> property for it.</p>\n\n<pre><code>// Writes '\\n' (e.g.).\nwriter.lineBreak();\n</code></pre>\n</div></div></div><div id='method-openTag' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.htmlWriter'>CKEDITOR.htmlWriter</span><br/><a href='source/plugin54.html#CKEDITOR-htmlWriter-method-openTag' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlWriter-method-openTag' class='name expandable'>openTag</a>( <span class='pre'>tagName, attributes</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Writes the tag opening part for an opener tag. ...</div><div class='long'><p>Writes the tag opening part for an opener tag.</p>\n\n<pre><code>// Writes '&lt;p'.\nwriter.openTag( 'p', { class : 'MyClass', id : 'MyId' } );\n</code></pre>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>tagName</span> : String<div class='sub-desc'><p>The element name for this tag.</p>\n</div></li><li><span class='pre'>attributes</span> : Object<div class='sub-desc'><p>The attributes defined for this tag. The\nattributes could be used to inspect the tag.</p>\n</div></li></ul><p>Overrides: <a href=\"#!/api/CKEDITOR.htmlParser.basicWriter-method-openTag\" rel=\"CKEDITOR.htmlParser.basicWriter-method-openTag\" class=\"docClass\">CKEDITOR.htmlParser.basicWriter.openTag</a></p></div></div></div><div id='method-openTagClose' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.htmlWriter'>CKEDITOR.htmlWriter</span><br/><a href='source/plugin54.html#CKEDITOR-htmlWriter-method-openTagClose' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlWriter-method-openTagClose' class='name expandable'>openTagClose</a>( <span class='pre'>tagName, isSelfClose</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Writes the tag closing part for an opener tag. ...</div><div class='long'><p>Writes the tag closing part for an opener tag.</p>\n\n<pre><code>// Writes '&gt;'.\nwriter.openTagClose( 'p', false );\n\n// Writes ' /&gt;'.\nwriter.openTagClose( 'br', true );\n</code></pre>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>tagName</span> : String<div class='sub-desc'><p>The element name for this tag.</p>\n</div></li><li><span class='pre'>isSelfClose</span> : Boolean<div class='sub-desc'><p>Indicates that this is a self-closing tag,\nlike <code>&lt;br&gt;</code> or <code>&lt;img&gt;</code>.</p>\n</div></li></ul><p>Overrides: <a href=\"#!/api/CKEDITOR.htmlParser.basicWriter-method-openTagClose\" rel=\"CKEDITOR.htmlParser.basicWriter-method-openTagClose\" class=\"docClass\">CKEDITOR.htmlParser.basicWriter.openTagClose</a></p></div></div></div><div id='method-reset' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.htmlWriter'>CKEDITOR.htmlWriter</span><br/><a href='source/plugin54.html#CKEDITOR-htmlWriter-method-reset' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlWriter-method-reset' class='name expandable'>reset</a>( <span class='pre'></span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Empties the current output buffer. ...</div><div class='long'><p>Empties the current output buffer. It also brings back the default\nvalues of the writer flags.</p>\n\n<pre><code>writer.reset();\n</code></pre>\n<p>Overrides: <a href=\"#!/api/CKEDITOR.htmlParser.basicWriter-method-reset\" rel=\"CKEDITOR.htmlParser.basicWriter-method-reset\" class=\"docClass\">CKEDITOR.htmlParser.basicWriter.reset</a></p></div></div></div><div id='method-setRules' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.htmlWriter'>CKEDITOR.htmlWriter</span><br/><a href='source/plugin54.html#CKEDITOR-htmlWriter-method-setRules' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlWriter-method-setRules' class='name expandable'>setRules</a>( <span class='pre'>tagName, rules</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Sets formatting rules for a given element. ...</div><div class='long'><p>Sets formatting rules for a given element. Possible rules are:</p>\n\n<ul>\n<li><code>indent</code> &ndash; indent the element content.</li>\n<li><code>breakBeforeOpen</code> &ndash; break line before the opener tag for this element.</li>\n<li><code>breakAfterOpen</code> &ndash; break line after the opener tag for this element.</li>\n<li><code>breakBeforeClose</code> &ndash; break line before the closer tag for this element.</li>\n<li><code>breakAfterClose</code> &ndash; break line after the closer tag for this element.</li>\n</ul>\n\n\n<p>All rules default to <code>false</code>. Each function call overrides rules that are\nalready present, leaving the undefined ones untouched.</p>\n\n<p>By default, all elements available in the <a href=\"#!/api/CKEDITOR.dtd-property-S-block\" rel=\"CKEDITOR.dtd-property-S-block\" class=\"docClass\">CKEDITOR.dtd.$block</a>,\n<a href=\"#!/api/CKEDITOR.dtd-property-S-listItem\" rel=\"CKEDITOR.dtd-property-S-listItem\" class=\"docClass\">CKEDITOR.dtd.$listItem</a>, and <a href=\"#!/api/CKEDITOR.dtd-property-S-tableContent\" rel=\"CKEDITOR.dtd-property-S-tableContent\" class=\"docClass\">CKEDITOR.dtd.$tableContent</a>\nlists have all the above rules set to <code>true</code>. Additionaly, the <code>&lt;br&gt;</code>\nelement has the <code>breakAfterOpen</code> rule set to <code>true</code>.</p>\n\n<pre><code>// Break line before and after \"img\" tags.\nwriter.setRules( 'img', {\n    breakBeforeOpen: true\n    breakAfterOpen: true\n} );\n\n// Reset the rules for the \"h1\" tag.\nwriter.setRules( 'h1', {} );\n</code></pre>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>tagName</span> : String<div class='sub-desc'><p>The name of the element for which the rules are set.</p>\n</div></li><li><span class='pre'>rules</span> : Object<div class='sub-desc'><p>An object containing the element rules.</p>\n</div></li></ul></div></div></div><div id='method-text' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.htmlWriter'>CKEDITOR.htmlWriter</span><br/><a href='source/plugin54.html#CKEDITOR-htmlWriter-method-text' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlWriter-method-text' class='name expandable'>text</a>( <span class='pre'>text</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Writes text. ...</div><div class='long'><p>Writes text.</p>\n\n<pre><code>// Writes 'Hello Word'.\nwriter.text( 'Hello Word' );\n</code></pre>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>text</span> : String<div class='sub-desc'><p>The text value</p>\n</div></li></ul><p>Overrides: <a href=\"#!/api/CKEDITOR.htmlParser.basicWriter-method-text\" rel=\"CKEDITOR.htmlParser.basicWriter-method-text\" class=\"docClass\">CKEDITOR.htmlParser.basicWriter.text</a></p></div></div></div><div id='method-write' class='member  inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><a href='#!/api/CKEDITOR.htmlParser.basicWriter' rel='CKEDITOR.htmlParser.basicWriter' class='defined-in docClass'>CKEDITOR.htmlParser.basicWriter</a><br/><a href='source/basicwriter.html#CKEDITOR-htmlParser-basicWriter-method-write' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.htmlParser.basicWriter-method-write' class='name expandable'>write</a>( <span class='pre'>data</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Writes any kind of data to the ouput. ...</div><div class='long'><p>Writes any kind of data to the ouput.</p>\n\n<pre><code>writer.write( 'This is an &lt;b&gt;example&lt;/b&gt;.' );\n</code></pre>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>data</span> : String<div class='sub-desc'>\n</div></li></ul></div></div></div></div></div></div></div>","meta":{}});