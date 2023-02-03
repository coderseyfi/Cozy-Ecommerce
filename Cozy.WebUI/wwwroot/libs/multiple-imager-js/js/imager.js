//author: Kamran A-eff
let noImage = `data:image/gif;base64,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`;

(function ($) {

    $.makeid = function (length) {
        var result = '';
        var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        var charactersLength = characters.length;
        for (var i = 0; i < length; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
        }
        return result;
    };

    $.fn.imgadd = function (options) {
        let that = this;

        options = $.extend({
            tnWidth: '120px',
            tnHeight: '120px',
            fontSize: '55px',
            plusInnerHtml: '<i class="fa fa-image"></i>',
            viewerWidth: '300px',
            viewerHeight: '300px',
            readonly: false
        }, options);

        $(that).each(function (index, element) {

            let ix = 1;
            let guid = `i${$.makeid(8)}`;
            let viewer = $(element).find('.viewer');
            let viewerThumbs = $(element).find('.viewer-thumbs');
            let elName = $(element).attr('name');

            if (viewer.length == 0) {
                viewer = $(`<div/>`, {
                    class: 'viewer'
                })
                    .css({
                        'background-image': `url(${noImage})`
                    });

                $(element).append(viewer);
            }

            if (viewerThumbs.length == 0) {
                viewerThumbs = $(`<div/>`, {
                    class: 'viewer-thumbs',
                    name: 'image'
                });

                $(element).append(viewerThumbs);
            }

            $(element).find('label[thumb-url]').each(function (thumbIndex, thumb) {
                $(viewer).removeClass('no-show');

                let url = $(thumb).attr('thumb-url');
                let imageId = $(thumb).attr('image-id');
                let checked = $(thumb).attr('checked') || false;

                viewerThumbs.append(thumb);

                $(thumb).addClass('img-thumb')
                    .attr('for', `${guid}${ix}`)
                    .removeAttr('thumb-url')
                    .removeAttr('image-id')
                    .removeAttr('checked')
                    .css({
                        width: options.tnWidth,
                        height: options.tnHeight,
                        'background-image': `url(${url})`
                    });

                if (checked == true) {
                    $(thumb).addClass('active');
                }

                $(thumb).click(function () {
                    $(viewer).css({
                        'background-image': `url(${url})`
                    });

                    $(element).find('.img-thumb')
                        .removeClass('active')
                        .find('[name$=".IsMain"]')
                        .prop('checked', false);

                    $(this).addClass('active')
                        .find('[name$=".IsMain"]')
                        .prop('checked', true);
                });

                var fileName = url.substring((url.lastIndexOf('/') == 0 ? -1 : url.lastIndexOf('/')) + 1);

                $(thumb).append($(`
                <input type='radio' name='${elName}[${ix - 1}].IsMain'   id='${guid}${ix} ${(checked == `checked` ? `checked` : ``)}'/>
                <input type='hidden'   name='${elName}[${ix - 1}].TempPath' value='${fileName}'/>
                <input type='hidden'   name='${elName}[${ix - 1}].Id'       value='${imageId}'/>
                ${(options.readonly == true ? `` : `<span class='remove-thumb'></span>`)}`));

                $(thumb).find('[class$="remove-thumb"]')
                    .click(function (e) {
                        e.stopPropagation();
                        e.preventDefault();

                        $(this).closest('.img-thumb')
                            .addClass('d-none')
                            .removeClass('active')
                            .find('[name$=".TempPath"]').val('');

                        if ($(that).find('.img-thumb.active').length == 0) {
                            let founded = $(that).find(`.img-thumb:not([class*='d-none']):not([for='${$(this).closest('.img-thumb').attr('for')}'])`)
                                .eq(0);

                            if (founded.length > 0)
                                $(founded).trigger('click');
                            else {
                                $(viewer).css({
                                    'background-image': `url(${noImage})`
                                })
                                    .addClass('no-show');
                            }
                        }
                    });

                if (checked == `checked`)
                    $(thumb).trigger('click');

                ix++;
            });

            viewer.css({
                width: options.viewerWidth,
                height: options.viewerHeight
            });

            if (options.readonly == true)
                return;

            let btnPlus = $(`<button class='img-plus' type='button'>${options.plusInnerHtml}</button>`)
                .css({
                    width: options.tnWidth,
                    height: options.tnHeight,
                });

            if (options.plusBtnClass) {
                $(btnPlus).addClass(options.plusBtnClass);
            }
            else {
                $(btnPlus).css({
                    'font-size': options.fontSize
                })
                    .addClass('img-plus-no-bt');
            }


            $(btnPlus).click(function () {
                let fileInput = $(`<input name='${elName}[${ix - 1}].File' type="file" accept="image/x-png,image/gif,image/jpeg"/>`);

                let label = $(`<label for='${guid}${ix}' class='img-thumb' style="background-image:url('libs/multiple-imager-js/img/img-rendering.gif')">
                                    <span class='remove-thumb'></span>
                               </label>`)
                    .append(fileInput)
                    .css({
                        width: options.tnWidth,
                        height: options.tnHeight
                    });

                $(element).find('.viewer-thumbs').append(label);

                $(label).append(`<input type='radio' name='${elName}[${ix - 1}].IsMain' id='${guid}${ix}'/>`);

                $(fileInput).change(function (e) {
                    $(label).attr('title', e.target.files[0].name);

                    let reader = new FileReader();
                    reader.addEventListener("load", function () {

                        $(label).css({
                            'background-image': `url(${reader.result})`
                        }).unbind('click')
                            .click(function () {

                                $(viewer).css({
                                    'background-image': `url(${reader.result})`
                                });

                                $(viewer).removeClass('no-show');

                                $(element).find('.img-thumb')
                                    .removeClass('active')
                                    .find('[name$=".IsMain"]')
                                    .prop('checked', false);

                                $(this).addClass('active')
                                    .find('[name$=".IsMain"]')
                                    .prop('checked', true);

                            });

                        $(label).find('[class$="remove-thumb"]')
                            .unbind('click')
                            .click(function (e) {
                                e.stopPropagation();
                                $(label).remove();

                                if ($(that).find('.img-thumb.active').length == 0) {
                                    let founded = $(that).find(`.img-thumb:not([class*='d-none']):not([for='${$(this).closest('.img-thumb').attr('for')}'])`)
                                        .eq(0);

                                    if (founded.length > 0)
                                        $(founded).trigger('click');
                                    else {
                                        $(viewer).css({
                                            'background-image': `url(${noImage})`
                                        });
                                    }
                                }
                            });

                        if ($(that).find('.img-thumb.active').length == 0)
                            $(label).trigger('click');

                    }, false);

                    reader.readAsDataURL(e.target.files[0]);
                });
                ix++;
                $(fileInput).trigger('click');
            });
            $(element).find('.viewer').append(btnPlus);
        });

    };


})(jQuery);