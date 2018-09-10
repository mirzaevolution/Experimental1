/*****EMPTY FUNCTION DETECTOR*****/
if (!Function.prototype.isFunctionEmpty) {
    Function.prototype.isFunctionEmpty =
        function () {
            var str = "";
            try {

                // Get content between first { and last }
                var m = this.toString().match(/\{([\s\S]*)\}/m)[1];
                // Strip comments
                str = m.replace(/^\s*\/\/.*$/mg, '').trim();
            } catch (ex) { str = "" }
            return str == "";
        };
}

(function ($) {
    $.fn.kmiUploader = function (options) {
        var element = $(this);
        var id = element.attr("id");
        var browse_button = id;
        var ref = {
            objRef: {},
            getFiles: function () {
                if (this.objRef) {
                    return this.objRef.files;
                }
            },
            getFile: function (id) {
                if (this.objRef) {
                    return this.objRef.getFile(id);
                }
            },
            removeFiles: function () {
                if (this.objRef) {
                    if (this.objRef.files && this.objRef.files.length > 0) {
                        while (this.objRef.files.length > 0) {
                            var file = this.objRef.files.pop();
                        }
                    }
                }
            },
            removeFile: function (id) {
                if (this.objRef) {
                    return this.objRef.removeFile(id);
                }
            },
            start: function () {
                if (this.objRef) {
                    this.objRef.start();
                }
            },
            stop: function () {
                if (this.objRef) {
                    this.objRef.stop();
                }
            },
            setOption: function (key, value) {
                if (key) {
                    switch (key.toLowerCase()) {
                        case "url": {
                            if (this.objRef) {
                                this.objRef.setOption("url", value);
                            }
                            break;
                        }
                        case "allowedextensions": {
                            if (this.objRef) {
                                this.objRef.setOption("mime_types", value);
                            }
                            break;
                        }
                        case "maxfilesize": {
                            if (this.objRef) {
                                this.objRef.setOption("max_file_size", value);
                            }
                            break;
                        }
                        case "issamefilenameallowed": {
                            if (this.objRef) {
                                this.objRef.setOption("prevent_duplicates", value);
                            }
                            break;
                        }
                        case "headers": {
                            if (this.objRef) {
                                this.objRef.setOption("headers", value);
                            }
                            break;
                        }
                        case "ismultipart": {
                            if (this.objRef) {
                                this.objRef.setOption("multipart", value);
                            }
                            break;
                        }
                        case "params": {
                            if (this.objRef) {
                                this.objRef.setOption("multipart_params", value);
                            }
                            break;
                        }
                        case "dropzone": {
                            if (this.objRef) {
                                this.objRef.setOption("drop_element", value);
                            }
                            break;
                        }
                        case "ismultiple": {
                            if (this.objRef) {
                                this.objRef.setOption("multi_selection", value);
                            }
                            break;
                        }
                        case "runtimes": {
                            if (this.objRef) {
                                this.objRef.setOption("runtimes", value);
                            }
                            break;
                        }
                        default: {
                            console.log("Invalid property key.");
                            break;
                        }
                    }
                }
            },
            getBaseOption: function () {
                if (this.objRef) {
                    return this.objRef.getOption();
                }
                return undefined;
            },
            getInfo: function () {
                if (this.objRef) {
                    return this.objRef.total;
                }
                return undefined;
            }
        };


        var settings = {};
        $.extend(true, settings, this.kmiUploader.defaults, options);

        var realSettings = {
            browse_button: browse_button,
            url: settings.url,
            filters: {
                mime_types: settings.allowedExtensions,
                max_file_size: settings.maxFileSize,
                prevent_duplicates: !settings.isSameFileNameAllowed
            },
            headers: settings.headers,
            multipart: settings.isMultipart,
            multipart_params: settings.params,
            drop_element: settings.dropzone,
            multi_selection: settings.isMultiple,
            runtimes: settings.runtimes
        };
        realSettings.init = {};
        if (settings.events.onPostInit && !settings.events.onPostInit.isFunctionEmpty()) {
            realSettings.init.PostInit = settings.events.onPostInit;
        }
        if (settings.events.onQueueChanged && !settings.events.onQueueChanged.isFunctionEmpty()) {
            realSettings.init.QueueChanged = settings.events.onQueueChanged;
        }
        if (settings.events.onFilesAdded && !settings.events.onFilesAdded.isFunctionEmpty()) {
            realSettings.init.FilesAdded = settings.events.onFilesAdded;
        }
        if (settings.events.onFilesRemoved && !settings.events.onFilesRemoved.isFunctionEmpty()) {
            realSettings.init.FilesRemoved = settings.events.onFilesRemoved;
        }
        if (settings.events.onBeforeUpload && !settings.events.onBeforeUpload.isFunctionEmpty()) {
            realSettings.init.BeforeUpload = settings.events.onBeforeUpload;
        }
        if (settings.events.onUploadFile && !settings.events.onUploadFile.isFunctionEmpty()) {
            realSettings.init.UploadFile = settings.events.onUploadFile;
        }
        if (settings.events.onUploadProgress && !settings.events.onUploadProgress.isFunctionEmpty()) {
            realSettings.init.UploadProgress = settings.events.onUploadProgress;
        }
        if (settings.events.onFileUploaded && !settings.events.onFileUploaded.isFunctionEmpty()) {
            realSettings.init.FileUploaded = settings.events.onFileUploaded;
        }
        if (settings.events.onUploadComplete && !settings.events.onUploadComplete.isFunctionEmpty()) {
            realSettings.init.UploadComplete = settings.events.onUploadComplete;
        }
        if (settings.events.onError && !settings.events.onError.isFunctionEmpty()) {
            realSettings.init.Error = settings.events.onError;
        }
        if (settings.events.onDestroy && !settings.events.onDestroy.isFunctionEmpty()) {
            realSettings.init.Destroy = settings.events.onDestroy;
        }

        var ___uploader___ = new plupload.Uploader(realSettings);
        ___uploader___.init();
        ref.objRef = ___uploader___;
        return ref;

    }


    $.fn.kmiUploader.defaults = {
        url: '',
        allowedExtensions: [],
        maxFileSize: 0,
        isSameFileNameAllowed: false,
        headers: undefined,
        isMultipart: true,
        params: undefined,
        dropzone: undefined,
        isMultiple: true,
        runtimes: 'html5,flash,silverlight,html4',

        events: {
            onPostInit: function (obj) { },
            onQueueChanged: function (obj) { },
            onFilesAdded: function (obj, files) { },
            onFilesRemoved: function (obj, files) { },
            onBeforeUpload: function (obj, file) { },
            onUploadFile: function (obj, file) { },
            onUploadProgress: function (obj, file) { },
            onFileUploaded: function (obj, file, result) { },
            onUploadComplete: function (obj, files) { },
            onError: function (obj, error) { },
            onDestroy: function (obj) { }
        }
    }
})(jQuery);