if (!window.jQuery) {
    alert('Jquery qosulmayib');
}
else {

    $(function () {
        $('.tab-control[role="tabcontrol"]')
            .each(function (tabControlIndex, tabControl) {

                let tabNavsContainer = $(`<ul/>`, {
                    class: 'tab-navs'
                });

                $(tabControl).append(tabNavsContainer);

                let tabPagesContainer = $(`<div/>`, {
                    class: 'tab-pages'
                });

                $(tabControl).append(tabPagesContainer);

                $(tabControl).find('.tab-page').each(function (tabPageIndex, tabPage) {
                    let options = {};
                    options.id = $(tabPage).attr('id') || 'x';
                    options.title = $(tabPage).attr('aria-title') || 'x';
                    options.selected = $(tabPage).attr('selected') != undefined;

                    $(tabPage).removeAttr('aria-title').removeAttr('selected');

                    $(tabPagesContainer).append(tabPage);

                    $(tabNavsContainer).append($(`<li><a href="#${options.id}" ${(options.selected ? 'aria-expanded="true"' : '')}>${options.title}</a></li>`));

                });

            });


    });

    $(function () {
        $('.tab-control').each(function (tabControlIndex, tabControl) {

            $(tabControl).find('.tab-navs a.active').removeClass('active');
            $(tabControl).find(`.tab-pages .tab-page.active`).removeClass('active');

            $(tabControl).find('.tab-navs a[aria-expanded="true"]')
                .each(function (tabNavLinkIndex, tabNavLink) {

                    let link = $(tabNavLink).attr('href');

                    if (link) {
                        $(tabNavLink).addClass('active');
                        $(tabControl).find(`.tab-pages .tab-page${link}`).addClass('active');
                    }
                });

            $(tabControl).find('.tab-navs a').click(function (e) {
                e.preventDefault();

                if ($(e.currentTarget).hasClass('active') == false) {
                    $(tabControl).find('.tab-navs a.active').removeClass('active');
                    $(tabControl).find(`.tab-pages .tab-page.active`).removeClass('active');

                    $(e.currentTarget).addClass('active');

                    let link = $(e.currentTarget).attr('href');
                    $(tabControl).find(`.tab-pages .tab-page${link}`).addClass('active');
                }

            });
        });

    });

    window.validateTabs = () => {
        $('.tab-page .field-validation-error').each(function (index, item) {
            let id = $(item).closest('').attr('id');
            $(`[href=#${id}]`).addClass('required');
        });
    }

}