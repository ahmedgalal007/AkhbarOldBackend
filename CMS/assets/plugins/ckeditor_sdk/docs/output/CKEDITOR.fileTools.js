Ext.data.JsonP.CKEDITOR_fileTools({"tagname":"class","name":"CKEDITOR.fileTools","autodetected":{},"files":[{"filename":"plugin.js","href":"plugin92.html#CKEDITOR-fileTools"}],"since":"4.5","singleton":true,"members":[{"name":"addUploadWidget","tagname":"method","owner":"CKEDITOR.fileTools","id":"method-addUploadWidget","meta":{}},{"name":"bindNotifications","tagname":"method","owner":"CKEDITOR.fileTools","id":"method-bindNotifications","meta":{}},{"name":"getUploadUrl","tagname":"method","owner":"CKEDITOR.fileTools","id":"method-getUploadUrl","meta":{}},{"name":"isTypeSupported","tagname":"method","owner":"CKEDITOR.fileTools","id":"method-isTypeSupported","meta":{}},{"name":"markElement","tagname":"method","owner":"CKEDITOR.fileTools","id":"method-markElement","meta":{}}],"alternateClassNames":[],"aliases":{},"id":"class-CKEDITOR.fileTools","component":false,"superclasses":[],"subclasses":[],"mixedInto":[],"mixins":[],"parentMixins":[],"requires":[],"uses":[],"html":"<div><pre class=\"hierarchy\"><h4>Files</h4><div class='dependency'><a href='source/plugin92.html#CKEDITOR-fileTools' target='_blank'>plugin.js</a></div></pre><div class='doc-contents'><p>Helpers to load and upload a file.</p>\n        <p>Available since: <b>4.5</b></p>\n</div><div class='members'><div class='members-section'><div class='definedBy'>Defined By</div><h3 class='members-title icon-method'>Methods</h3><div class='subsection'><div id='method-addUploadWidget' class='member first-child not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.fileTools'>CKEDITOR.fileTools</span><br/><a href='source/plugin94.html#CKEDITOR-fileTools-method-addUploadWidget' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.fileTools-method-addUploadWidget' class='name expandable'>addUploadWidget</a>( <span class='pre'>editor, name, def</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>This function creates an upload widget &mdash; a placeholder to show the progress of an upload. ...</div><div class='long'><p>This function creates an upload widget &mdash; a placeholder to show the progress of an upload. The upload widget\nis based on its <a href=\"#!/api/CKEDITOR.fileTools.uploadWidgetDefinition\" rel=\"CKEDITOR.fileTools.uploadWidgetDefinition\" class=\"docClass\">definition</a>. The <code>addUploadWidget</code> method also\ncreates a <code>paste</code> event, if the <a href=\"#!/api/CKEDITOR.fileTools.uploadWidgetDefinition-property-fileToElement\" rel=\"CKEDITOR.fileTools.uploadWidgetDefinition-property-fileToElement\" class=\"docClass\">fileToElement</a> method\nis defined. This event helps in handling pasted files, as it will automatically check if the files were pasted and\nmark them to be uploaded.</p>\n\n<p>The upload widget helps to handle content that is uploaded asynchronously inside the editor. It solves issues such as:\nediting during upload, undo manager integration, getting data, removing or copying uploaded element.</p>\n\n<p>To create an upload widget you need to define two transformation methods:</p>\n\n<ul>\n<li>The <a href=\"#!/api/CKEDITOR.fileTools.uploadWidgetDefinition-property-fileToElement\" rel=\"CKEDITOR.fileTools.uploadWidgetDefinition-property-fileToElement\" class=\"docClass\">fileToElement</a> method which will be called on\n<code>paste</code> and transform a file into a placeholder.</li>\n<li>The <a href=\"#!/api/CKEDITOR.fileTools.uploadWidgetDefinition-property-onUploaded\" rel=\"CKEDITOR.fileTools.uploadWidgetDefinition-property-onUploaded\" class=\"docClass\">onUploaded</a> method with\nthe <a href=\"#!/api/CKEDITOR.fileTools.uploadWidgetDefinition-property-replaceWith\" rel=\"CKEDITOR.fileTools.uploadWidgetDefinition-property-replaceWith\" class=\"docClass\">replaceWith</a> method which will be called to replace\nthe upload placeholder with the final HTML when the upload is done.\nIf you want to show more information about the progress you can also define\nthe <a href=\"#!/api/CKEDITOR.fileTools.uploadWidgetDefinition-property-onLoading\" rel=\"CKEDITOR.fileTools.uploadWidgetDefinition-property-onLoading\" class=\"docClass\">onLoading</a> and\n<a href=\"#!/api/CKEDITOR.fileTools.uploadWidgetDefinition-property-onUploading\" rel=\"CKEDITOR.fileTools.uploadWidgetDefinition-property-onUploading\" class=\"docClass\">onUploading</a> methods.</li>\n</ul>\n\n\n<p>The simplest uploading widget which uploads a file and creates a link to it may look like this:</p>\n\n<pre><code>    <a href=\"#!/api/CKEDITOR.fileTools-method-addUploadWidget\" rel=\"CKEDITOR.fileTools-method-addUploadWidget\" class=\"docClass\">CKEDITOR.fileTools.addUploadWidget</a>( editor, 'uploadfile', {\n    uploadUrl: <a href=\"#!/api/CKEDITOR.fileTools-method-getUploadUrl\" rel=\"CKEDITOR.fileTools-method-getUploadUrl\" class=\"docClass\">CKEDITOR.fileTools.getUploadUrl</a>( editor.config ),\n\n    fileToElement: function( file ) {\n        var a = new <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a>( 'a' );\n        a.setText( file.name );\n        a.setAttribute( 'href', '#' );\n        return a;\n    },\n\n    onUploaded: function( upload ) {\n        this.replaceWith( '&lt;a href=\"' + upload.url + '\" target=\"_blank\"&gt;' + upload.fileName + '&lt;/a&gt;' );\n    }\n} );\n</code></pre>\n\n<p>The upload widget uses <a href=\"#!/api/CKEDITOR.fileTools.fileLoader\" rel=\"CKEDITOR.fileTools.fileLoader\" class=\"docClass\">CKEDITOR.fileTools.fileLoader</a> as a helper to upload the file. A\n<a href=\"#!/api/CKEDITOR.fileTools.fileLoader\" rel=\"CKEDITOR.fileTools.fileLoader\" class=\"docClass\">CKEDITOR.fileTools.fileLoader</a> instance is created when the file is pasted and a proper method is\ncalled &mdash; by default it is the <a href=\"#!/api/CKEDITOR.fileTools.fileLoader-method-loadAndUpload\" rel=\"CKEDITOR.fileTools.fileLoader-method-loadAndUpload\" class=\"docClass\">CKEDITOR.fileTools.fileLoader.loadAndUpload</a> method. If you want\nto only use the <code>load</code> or <code>upload</code>, you can use the <a href=\"#!/api/CKEDITOR.fileTools.uploadWidgetDefinition-property-loadMethod\" rel=\"CKEDITOR.fileTools.uploadWidgetDefinition-property-loadMethod\" class=\"docClass\">loadMethod</a>\nproperty.</p>\n\n<p>Note that if you want to handle a big file, e.g. a video, you may need to use <code>upload</code> instead of\n<code>loadAndUpload</code> because the file may be too big to load it to memory at once.</p>\n\n<p>If you do not upload the file, you need to define <a href=\"#!/api/CKEDITOR.fileTools.uploadWidgetDefinition-property-onLoaded\" rel=\"CKEDITOR.fileTools.uploadWidgetDefinition-property-onLoaded\" class=\"docClass\">onLoaded</a>\ninstead of <a href=\"#!/api/CKEDITOR.fileTools.uploadWidgetDefinition-property-onUploaded\" rel=\"CKEDITOR.fileTools.uploadWidgetDefinition-property-onUploaded\" class=\"docClass\">onUploaded</a>.\nFor example, if you want to read the content of the file:</p>\n\n<pre><code><a href=\"#!/api/CKEDITOR.fileTools-method-addUploadWidget\" rel=\"CKEDITOR.fileTools-method-addUploadWidget\" class=\"docClass\">CKEDITOR.fileTools.addUploadWidget</a>( editor, 'fileReader', {\n    loadMethod: 'load',\n    supportedTypes: /text\\/(plain|html)/,\n\n    fileToElement: function( file ) {\n        var el = new <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a>( 'span' );\n        el.setText( '...' );\n        return el;\n    },\n\n    onLoaded: function( loader ) {\n        this.replaceWith( atob( loader.data.split( ',' )[ 1 ] ) );\n    }\n} );\n</code></pre>\n\n<p>If you need to pass additional data to the request, you can do this using the\n<a href=\"#!/api/CKEDITOR.fileTools.uploadWidgetDefinition-property-additionalRequestParameters\" rel=\"CKEDITOR.fileTools.uploadWidgetDefinition-property-additionalRequestParameters\" class=\"docClass\">additionalRequestParameters</a> property.\nThat data is then passed to the upload method defined by <a href=\"#!/api/CKEDITOR.fileTools.uploadWidgetDefinition-property-loadMethod\" rel=\"CKEDITOR.fileTools.uploadWidgetDefinition-property-loadMethod\" class=\"docClass\">CKEDITOR.fileTools.uploadWidgetDefinition.loadMethod</a>,\nand to the <a href=\"#!/api/CKEDITOR.editor-event-fileUploadRequest\" rel=\"CKEDITOR.editor-event-fileUploadRequest\" class=\"docClass\">CKEDITOR.editor.fileUploadRequest</a> event (as part of the <code>requestData</code> property).\nThe syntax of that parameter is compatible with the <a href=\"#!/api/CKEDITOR.editor-event-fileUploadRequest\" rel=\"CKEDITOR.editor-event-fileUploadRequest\" class=\"docClass\">CKEDITOR.editor.fileUploadRequest</a> <code>requestData</code> property.</p>\n\n<pre><code><a href=\"#!/api/CKEDITOR.fileTools-method-addUploadWidget\" rel=\"CKEDITOR.fileTools-method-addUploadWidget\" class=\"docClass\">CKEDITOR.fileTools.addUploadWidget</a>( editor, 'uploadFile', {\n    additionalRequestParameters: {\n        foo: 'bar'\n    },\n\n    fileToElement: function( file ) {\n        var el = new <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a>( 'span' );\n        el.setText( '...' );\n        return el;\n    },\n\n    onUploaded: function( upload ) {\n        this.replaceWith( '&lt;a href=\"' + upload.url + '\" target=\"_blank\"&gt;' + upload.fileName + '&lt;/a&gt;' );\n    }\n} );\n</code></pre>\n\n<p>If you need custom <code>paste</code> handling, you need to mark the pasted element to be changed into an upload widget\nusing <a href=\"#!/api/CKEDITOR.fileTools-method-markElement\" rel=\"CKEDITOR.fileTools-method-markElement\" class=\"docClass\">markElement</a>. For example, instead of the <code>fileToElement</code> helper from the\nexample above, a <code>paste</code> listener can be created manually:</p>\n\n<pre><code>editor.on( 'paste', function( evt ) {\n    var file, i, el, loader;\n\n    for ( i = 0; i &lt; evt.data.dataTransfer.getFilesCount(); i++ ) {\n        file = evt.data.dataTransfer.getFile( i );\n\n        if ( <a href=\"#!/api/CKEDITOR.fileTools-method-isTypeSupported\" rel=\"CKEDITOR.fileTools-method-isTypeSupported\" class=\"docClass\">CKEDITOR.fileTools.isTypeSupported</a>( file, /text\\/(plain|html)/ ) ) {\n            el = new <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a>( 'span' ),\n            loader = editor.uploadRepository.create( file );\n\n            el.setText( '...' );\n\n            loader.load();\n\n            <a href=\"#!/api/CKEDITOR.fileTools-method-markElement\" rel=\"CKEDITOR.fileTools-method-markElement\" class=\"docClass\">CKEDITOR.fileTools.markElement</a>( el, 'filereader', loader.id );\n\n            evt.data.dataValue += el.getOuterHtml();\n        }\n    }\n} );\n</code></pre>\n\n<p>Note that you can bind notifications to the upload widget on paste using\nthe <a href=\"#!/api/CKEDITOR.fileTools-method-bindNotifications\" rel=\"CKEDITOR.fileTools-method-bindNotifications\" class=\"docClass\">bindNotifications</a> method, so notifications will automatically\nshow the progress of the upload. Because this method shows notifications about upload, do not use it if you only\n<a href=\"#!/api/CKEDITOR.fileTools.fileLoader-method-load\" rel=\"CKEDITOR.fileTools.fileLoader-method-load\" class=\"docClass\">load</a> (and not upload) a file.</p>\n\n<pre><code>editor.on( 'paste', function( evt ) {\n    var file, i, el, loader;\n\n    for ( i = 0; i &lt; evt.data.dataTransfer.getFilesCount(); i++ ) {\n        file = evt.data.dataTransfer.getFile( i );\n\n        if ( <a href=\"#!/api/CKEDITOR.fileTools-method-isTypeSupported\" rel=\"CKEDITOR.fileTools-method-isTypeSupported\" class=\"docClass\">CKEDITOR.fileTools.isTypeSupported</a>( file, /text\\/pdf/ ) ) {\n            el = new <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a>( 'span' ),\n            loader = editor.uploadRepository.create( file );\n\n            el.setText( '...' );\n\n            loader.upload();\n\n            <a href=\"#!/api/CKEDITOR.fileTools-method-markElement\" rel=\"CKEDITOR.fileTools-method-markElement\" class=\"docClass\">CKEDITOR.fileTools.markElement</a>( el, 'pdfuploader', loader.id );\n\n            <a href=\"#!/api/CKEDITOR.fileTools-method-bindNotifications\" rel=\"CKEDITOR.fileTools-method-bindNotifications\" class=\"docClass\">CKEDITOR.fileTools.bindNotifications</a>( editor, loader );\n\n            evt.data.dataValue += el.getOuterHtml();\n        }\n    }\n} );\n</code></pre>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>editor</span> : <a href=\"#!/api/CKEDITOR.editor\" rel=\"CKEDITOR.editor\" class=\"docClass\">CKEDITOR.editor</a><div class='sub-desc'><p>The editor instance.</p>\n</div></li><li><span class='pre'>name</span> : String<div class='sub-desc'><p>The name of the upload widget.</p>\n</div></li><li><span class='pre'>def</span> : <a href=\"#!/api/CKEDITOR.fileTools.uploadWidgetDefinition\" rel=\"CKEDITOR.fileTools.uploadWidgetDefinition\" class=\"docClass\">CKEDITOR.fileTools.uploadWidgetDefinition</a><div class='sub-desc'><p>Upload widget definition.</p>\n</div></li></ul></div></div></div><div id='method-bindNotifications' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.fileTools'>CKEDITOR.fileTools</span><br/><a href='source/plugin94.html#CKEDITOR-fileTools-method-bindNotifications' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.fileTools-method-bindNotifications' class='name expandable'>bindNotifications</a>( <span class='pre'>editor, loader</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Binds a notification to the file loader so the upload widget will use\nthe notification to show the status and progress. ...</div><div class='long'><p>Binds a notification to the <a href=\"#!/api/CKEDITOR.fileTools.fileLoader\" rel=\"CKEDITOR.fileTools.fileLoader\" class=\"docClass\">file loader</a> so the upload widget will use\nthe notification to show the status and progress.\nThis function uses <a href=\"#!/api/CKEDITOR.plugins.notificationAggregator\" rel=\"CKEDITOR.plugins.notificationAggregator\" class=\"docClass\">CKEDITOR.plugins.notificationAggregator</a>, so even if multiple files are uploading\nonly one notification is shown. Warnings are an exception, because they are shown in separate notifications.\nThis notification shows only the progress of the upload, so this method should not be used if\nthe <a href=\"#!/api/CKEDITOR.fileTools.fileLoader-method-load\" rel=\"CKEDITOR.fileTools.fileLoader-method-load\" class=\"docClass\">loader.load</a> method was called. It works with\n<a href=\"#!/api/CKEDITOR.fileTools.fileLoader-method-upload\" rel=\"CKEDITOR.fileTools.fileLoader-method-upload\" class=\"docClass\">upload</a> and <a href=\"#!/api/CKEDITOR.fileTools.fileLoader-method-loadAndUpload\" rel=\"CKEDITOR.fileTools.fileLoader-method-loadAndUpload\" class=\"docClass\">loadAndUpload</a>.</p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>editor</span> : <a href=\"#!/api/CKEDITOR.editor\" rel=\"CKEDITOR.editor\" class=\"docClass\">CKEDITOR.editor</a><div class='sub-desc'><p>The editor instance.</p>\n</div></li><li><span class='pre'>loader</span> : <a href=\"#!/api/CKEDITOR.fileTools.fileLoader\" rel=\"CKEDITOR.fileTools.fileLoader\" class=\"docClass\">CKEDITOR.fileTools.fileLoader</a><div class='sub-desc'><p>The file loader instance.</p>\n</div></li></ul></div></div></div><div id='method-getUploadUrl' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.fileTools'>CKEDITOR.fileTools</span><br/><a href='source/plugin92.html#CKEDITOR-fileTools-method-getUploadUrl' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.fileTools-method-getUploadUrl' class='name expandable'>getUploadUrl</a>( <span class='pre'>config, [type]</span> ) : String/null<span class=\"signature\"></span></div><div class='description'><div class='short'>Gets the upload URL from the configuration. ...</div><div class='long'><p>Gets the upload URL from the <a href=\"#!/api/CKEDITOR.config\" rel=\"CKEDITOR.config\" class=\"docClass\">configuration</a>. Because of backward compatibility\nthe URL can be set using multiple configuration options.</p>\n\n<p>If the <code>type</code> is defined, then four configuration options will be checked in the following order\n(examples for <code>type='image'</code>):</p>\n\n<ul>\n<li><code>[type]UploadUrl</code>, e.g. <a href=\"#!/api/CKEDITOR.config-cfg-imageUploadUrl\" rel=\"CKEDITOR.config-cfg-imageUploadUrl\" class=\"docClass\">CKEDITOR.config.imageUploadUrl</a>,</li>\n<li><a href=\"#!/api/CKEDITOR.config-cfg-uploadUrl\" rel=\"CKEDITOR.config-cfg-uploadUrl\" class=\"docClass\">CKEDITOR.config.uploadUrl</a>,</li>\n<li><code>filebrowser[uppercased type]uploadUrl</code>, e.g. <a href=\"#!/api/CKEDITOR.config-cfg-filebrowserImageUploadUrl\" rel=\"CKEDITOR.config-cfg-filebrowserImageUploadUrl\" class=\"docClass\">CKEDITOR.config.filebrowserImageUploadUrl</a>,</li>\n<li><a href=\"#!/api/CKEDITOR.config-cfg-filebrowserUploadUrl\" rel=\"CKEDITOR.config-cfg-filebrowserUploadUrl\" class=\"docClass\">CKEDITOR.config.filebrowserUploadUrl</a>.</li>\n</ul>\n\n\n<p>If the <code>type</code> is not defined, two configuration options will be checked:</p>\n\n<ul>\n<li><a href=\"#!/api/CKEDITOR.config-cfg-uploadUrl\" rel=\"CKEDITOR.config-cfg-uploadUrl\" class=\"docClass\">CKEDITOR.config.uploadUrl</a>,</li>\n<li><a href=\"#!/api/CKEDITOR.config-cfg-filebrowserUploadUrl\" rel=\"CKEDITOR.config-cfg-filebrowserUploadUrl\" class=\"docClass\">CKEDITOR.config.filebrowserUploadUrl</a>.</li>\n</ul>\n\n\n<p><code>filebrowser[type]uploadUrl</code> and <code>filebrowserUploadUrl</code> are checked for backward compatibility with the\n<code>filebrowser</code> plugin.</p>\n\n<p>For both <code>filebrowser[type]uploadUrl</code> and <code>filebrowserUploadUrl</code> <code>&amp;responseType=json</code> is added to the end of the URL.</p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>config</span> : Object<div class='sub-desc'><p>The configuration file.</p>\n</div></li><li><span class='pre'>type</span> : String (optional)<div class='sub-desc'><p>Upload file type.</p>\n</div></li></ul><h3 class='pa'>Returns</h3><ul><li><span class='pre'>String/null</span><div class='sub-desc'><p>Upload URL or <code>null</code> if none of the configuration options were defined.</p>\n</div></li></ul></div></div></div><div id='method-isTypeSupported' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.fileTools'>CKEDITOR.fileTools</span><br/><a href='source/plugin92.html#CKEDITOR-fileTools-method-isTypeSupported' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.fileTools-method-isTypeSupported' class='name expandable'>isTypeSupported</a>( <span class='pre'>file, supportedTypes</span> ) : Boolean<span class=\"signature\"></span></div><div class='description'><div class='short'>Checks if the MIME type of the given file is supported. ...</div><div class='long'><p>Checks if the MIME type of the given file is supported.</p>\n\n<pre><code>    <a href=\"#!/api/CKEDITOR.fileTools-method-isTypeSupported\" rel=\"CKEDITOR.fileTools-method-isTypeSupported\" class=\"docClass\">CKEDITOR.fileTools.isTypeSupported</a>( { type: 'image/png' }, /image\\/(png|jpeg)/ ); // true\n    <a href=\"#!/api/CKEDITOR.fileTools-method-isTypeSupported\" rel=\"CKEDITOR.fileTools-method-isTypeSupported\" class=\"docClass\">CKEDITOR.fileTools.isTypeSupported</a>( { type: 'image/png' }, /image\\/(gif|jpeg)/ ); // false\n</code></pre>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>file</span> : Blob<div class='sub-desc'><p>The file to check.</p>\n</div></li><li><span class='pre'>supportedTypes</span> : RegExp<div class='sub-desc'><p>A regular expression to check the MIME type of the file.</p>\n</div></li></ul><h3 class='pa'>Returns</h3><ul><li><span class='pre'>Boolean</span><div class='sub-desc'><p><code>true</code> if the file type is supported.</p>\n</div></li></ul></div></div></div><div id='method-markElement' class='member  not-inherited'><a href='#' class='side expandable'><span>&nbsp;</span></a><div class='title'><div class='meta'><span class='defined-in' rel='CKEDITOR.fileTools'>CKEDITOR.fileTools</span><br/><a href='source/plugin94.html#CKEDITOR-fileTools-method-markElement' target='_blank' class='view-source'>view source</a></div><a href='#!/api/CKEDITOR.fileTools-method-markElement' class='name expandable'>markElement</a>( <span class='pre'>element, widgetName, loaderId</span> )<span class=\"signature\"></span></div><div class='description'><div class='short'>Marks an element which should be transformed into an upload widget. ...</div><div class='long'><p>Marks an element which should be transformed into an upload widget.</p>\n\n<p><a href=\"#!/api/CKEDITOR.fileTools-method-addUploadWidget\" rel=\"CKEDITOR.fileTools-method-addUploadWidget\" class=\"docClass\">CKEDITOR.fileTools.addUploadWidget</a></p>\n<h3 class=\"pa\">Parameters</h3><ul><li><span class='pre'>element</span> : <a href=\"#!/api/CKEDITOR.dom.element\" rel=\"CKEDITOR.dom.element\" class=\"docClass\">CKEDITOR.dom.element</a><div class='sub-desc'><p>Element to be marked.</p>\n</div></li><li><span class='pre'>widgetName</span> : String<div class='sub-desc'><p>The name of the upload widget.</p>\n</div></li><li><span class='pre'>loaderId</span> : Number<div class='sub-desc'><p>The ID of a related <a href=\"#!/api/CKEDITOR.fileTools.fileLoader\" rel=\"CKEDITOR.fileTools.fileLoader\" class=\"docClass\">CKEDITOR.fileTools.fileLoader</a>.</p>\n</div></li></ul></div></div></div></div></div></div></div>","meta":{}});