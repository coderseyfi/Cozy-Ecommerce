// preloader
//---------------------------------------------------------------------------
$(window).on('load', function () {
    $('#preloader').fadeOut('slow');
});

//header start
$(document).ready(function () {


    const header = document.querySelector(".header-top")
    const agota = document.querySelector(".header-top__container__left__logo__link__img")
    const links = [...document.querySelectorAll(".same-color")]
    const menuIcon = document.querySelector(".header-top__container__left__bar__link__icon")
    const xIcon = document.querySelector('.close-icon')
    const backTop = document.querySelector('.top')
    let sticky = header.offsetTop;

    window.addEventListener('scroll', () => {
        links.forEach((item) => {
            if (header && agota) {
                if (window.pageYOffset - 20 > sticky) {
                    header.classList.add("sticky")
                    item.style.color = "black"
                    agota.style.filter = "invert(1%)"
                    menuIcon.style.color = "black"
                    $('.active-line').addClass('active-line-sticky')
                } else {
                    header.classList.remove("sticky");
                    item.style.color = "white"
                    agota.style.filter = "invert(1)"
                    menuIcon.style.color = "white"
                    $('.active-line').removeClass('active-line-sticky')
                }
            }
        })
    })



    if ($('.top')) {
        $(window).on("scroll", function () {
            $(window).scrollTop() > 350 ? $(".top").fadeIn(650) : $(".top").fadeOut(550);
        }), $(".top").on("click", function () {
            $("html,body").animate({
                scrollTop: 0
            });
        });
    }


    //bestseller section
    const products = [...document.querySelectorAll(".product")]
    if (products) {
        products.forEach((item) => {
            item.addEventListener('mouseenter', (e) => {
                e.target.querySelector(".product__image__button").style.opacity = "1"
                e.target.querySelector(".product__image__link--img").style.opacity = "0"
                e.target.querySelector(".product__image__hover--img").style.opacity = "1"
            })
            item.addEventListener('mouseleave', (e) => {
                e.target.querySelector(".product__image__button").style.opacity = "0"
                e.target.querySelector(".product__image__link--img").style.opacity = "1"
                e.target.querySelector(".product__image__hover--img").style.opacity = "0"
            })
        })
    }



    const productsShop = [...document.querySelectorAll(".single-product")]
    if (productsShop) {
        productsShop.forEach((item) => {
            item.addEventListener('mouseenter', (e) => {
                e.target.querySelector(".single-product-hover-img a img").style.opacity = "1"
                e.target.querySelector(".single-product-img a img").style.opacity = "0"
                e.target.querySelector(".single-product-button").style.opacity = "1"
            })
            item.addEventListener('mouseleave', (e) => {
                e.target.querySelector(".single-product-hover-img a img").style.opacity = "0"
                e.target.querySelector(".single-product-img a img").style.opacity = "1"
                e.target.querySelector(".single-product-button").style.opacity = "0"
            })
        })
    }


    if ($('.autoplay')) {
        $('.autoplay').slick({
            slidesToShow: 4,
            slidesToScroll: 1,
            // autoplay: true,
            autoplaySpeed: 1000,
            arrows: false,
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 1,
                    },
                },
                {
                    breakpoint: 992,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 1,
                    },
                },
                {
                    breakpoint: 576,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                    },
                },

            ]

        });
    }


    if ($('.blog-slider')) {
        $('.blog-slider').slick({
            slidesToShow: 3,
            slidesToScroll: 1,
            // autoplay: true,
            autoplaySpeed: 2000,
            arrows: false,
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 1,
                    },
                },
                {
                    breakpoint: 992,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 1,
                    },
                },
                {
                    breakpoint: 576,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                    },
                },

            ]
        });
    }


    if ($('.brand-active')) {
        $('.brand-active').slick({
            slidesToShow: 5,
            slidesToScroll: 1,
            // autoplay: true,
            autoplaySpeed: 2000,
            arrows: false,
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 5,
                        slidesToScroll: 1,
                    },
                },
                {
                    breakpoint: 992,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 1,
                    },
                },
                {
                    breakpoint: 576,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 1,
                    },
                },
                {
                    breakpoint: 479,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                    },
                },

            ]
        });
    }


    if ($('.testimonial-active')) {
        $('.testimonial-active').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            // autoplay: true,
            autoplaySpeed: 2000,
            arrows: true,
            prevArrow: $('.testimonial-area .prev'),
            nextArrow: $('.testimonial-area .next'),
            appendDots:$(".testimonial--dots"),
            responsive: [
                {
                    breakpoint: 1400,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        
                    },
                },
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                    },
                },
                {
                    breakpoint: 992,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                    },
                },
                {
                    breakpoint: 576,
                    settings: {
                        arrows: false,
                        dots: true,
                        slidesToShow: 1,
                        slidesToScroll: 1,
                    },
                },

            ]
        });
    }



    $('.header').mouseover(() => {
        $('.header a').css('color', 'black')
        $(agota).css('filter', 'invert(0)')
        $(menuIcon).css('color', 'black')
    })


    $('.header').mouseleave(() => {
        $('.header a').css('color', 'white')
        $(agota).css('filter', 'invert(1)')
        $(menuIcon).css('color', 'white')
        if (window.pageYOffset - 20 > sticky) {
            $(agota).css('filter', 'invert(0)')
            $('.header a').css('color', 'black')
            $(menuIcon).css('color', 'black')
        }
    })


    var pathname = location.pathname
    $(".same-color").each(function () {
        if ($(this).attr('href') == pathname) {
            $(this).addClass('active-line')
        }
    })


    $(menuIcon).click(() => {
        $('.side-mobile-menu').css('left', '0')
        $('.body-overlay').addClass('opened')
    });


    $(xIcon).click(() => {
        $('.side-mobile-menu').css('left', '-200%')
        $('.body-overlay').removeClass('opened')
    });


    $(".header-top__container__right__item__link").click(() => {
        $('.shopping-details').css('right', '0')
    })


    $(".close-icon-shop").click(() => {
        $('.shopping-details').css('right', '-205%')
    })


    // mobile-menu-sidebar
    //---------------------------------------------------------------------------
    $(".mobile-menubar").on("click", function () {
        $(".side-mobile-menu").addClass('open-menubar');
        $(".body-overlay").addClass("opened");
    });


    $(".close-icon").click(function () {
        $(".side-mobile-menu").removeClass('open-menubar');
        $(".body-overlay").removeClass("opened");
    });


    $(".body-overlay").on("click", function () {
        $(".side-mobile-menu").css("left","-200%");
        $(".body-overlay").removeClass("opened");
    });


    //search-details start
    //------------------------------------
    $('.search-icon-header').on('click', () => {
        $('.header-search-details').addClass('search-active')
    })


    $('.close-icon-search').on('click', () => {
        $('.header-search-details').removeClass('search-active')
    })


    $('.eye').on('click',()=>{
        $('.modal').css('display','block')
        $('body').addClass('modal-open')
        $('.modal-back').addClass('not-fade')
    })


    $('.close-modal').on('click',()=>{
        $('.modal').css('display','none')
        $('body').removeClass('modal-open')
        $('.modal-back').removeClass('not-fade')
    })
    

    let tabs = document.querySelectorAll('.single-pr'),
        contents = document.querySelectorAll('.arm')
        
    tabs.forEach((tab,index)=>{
        tab.addEventListener('click',()=>{
            contents.forEach((content)=>{
                content.classList.remove('active')
            });
            tabs.forEach((tab)=>{
                tab.classList.remove('nav-link-active')
            })

            contents[index].classList.add('active')
            tabs[index].classList.add('nav-link-active')
        })
    })


    /* Price filter active 2*/
    //---------------------------------------------------------------------------
    if ($("#slider-range1").length) {
        $("#slider-range1").slider({
            range: true,
            min: 90,
            max: 1140,
            values: [90, 1140],
            slide: function (event, ui) {
                $("#amount1").val("$" + ui.values[0] + " - $" + ui.values[1]);
            }
        });
        $("#amount1").val("$" + $("#slider-range1").slider("values", 0) +
            " - $" + $("#slider-range1").slider("values", 1));


        $('#filter-btn').on('click', function () {
            $('.filter-widget').slideToggle(1000);
        });
    }


     /* Price filter active 2*/
    //---------------------------------------------------------------------------
    if ($("#slider-range2").length) {
        $("#slider-range2").slider({
            range: true,
            min: 90,
            max: 1140,
            values: [90, 1140],
            slide: function (event, ui) {
                $("#amount2").val("$" + ui.values[0] + " - $" + ui.values[1]);
            }
        });
        $("#amount2").val("$" + $("#slider-range2").slider("values", 0) +
            " - $" + $("#slider-range2").slider("values", 1));


        $('#filter-btn').on('click', function () {
            $('.filter-widget').slideToggle(1000);
        });
    }


    if($('.product-filter-details')){
        $('.product-filter-btn').on('click',()=>{
            $('.product-filter-details').toggleClass('active-filter')
        })
    }


    $('.custom-next').on('click',()=>{
      let inpVal = Number($('.quantity-input-arrow').val())
      $('.quantity-input-arrow').val(inpVal+1)
    })


    $('.custom-prev').on('click',()=>{
        let inpVal = Number($('.quantity-input-arrow').val())
        if (inpVal <=0) {
            return; 
        }
        $('.quantity-input-arrow').val(inpVal-1)
    })

    

    var QtyInput = (function () {
        var $qtyInputs = $(".qty-input");
    
        if (!$qtyInputs.length) {
            return;
        }
    
        var $inputs = $qtyInputs.find(".product-qty");
        var $countBtn = $qtyInputs.find(".qty-count");
        var qtyMin = parseInt($inputs.attr("min"));
        var qtyMax = parseInt($inputs.attr("max"));
    
        $inputs.change(function () {
            var $this = $(this);
            var $minusBtn = $this.siblings(".qty-count--minus");
            var $addBtn = $this.siblings(".qty-count--add");
            var qty = parseInt($this.val());
    
            if (isNaN(qty) || qty <= qtyMin) {
                $this.val(qtyMin);
                $minusBtn.attr("disabled", true);
            } else {
                $minusBtn.attr("disabled", false);
                
                if(qty >= qtyMax){
                    $this.val(qtyMax);
                    $addBtn.attr('disabled', true);
                } else {
                    $this.val(qty);
                    $addBtn.attr('disabled', false);
                }
            }
        });
    
        $countBtn.click(function () {
            var operator = this.dataset.action;
            var $this = $(this);
            var $input = $this.siblings(".product-qty");
            var qty = parseInt($input.val());
    
            if (operator == "add") {
                qty += 1;
                if (qty >= qtyMin + 1) {
                    $this.siblings(".qty-count--minus").attr("disabled", false);
                }
    
                if (qty >= qtyMax) {
                    $this.attr("disabled", true);
                }
            } else {
                qty = qty <= qtyMin ? qtyMin : (qty -= 1);
                
                if (qty == qtyMin) {
                    $this.attr("disabled", true);
                }
    
                if (qty < qtyMax) {
                    $this.siblings(".qty-count--add").attr("disabled", false);
                }
            }
    
            $input.val(qty);
        });
    })();
    
   




    let modalPrs = document.querySelectorAll('.m-pr'),
        mImgs = document.querySelectorAll('.m-img')
        
        modalPrs.forEach((modal,index)=>{
            modal.addEventListener('click',()=>{
            mImgs.forEach((img)=>{
                img.classList.remove('img-active')
            });
            modalPrs.forEach((modal)=>{
                modal.classList.remove('nav-link-active')
            })

            mImgs[index].classList.add('img-active')
            modalPrs[index].classList.add('nav-link-active')
        })
    })

});


