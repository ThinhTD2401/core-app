var ProductDetailController = function () {
    this.initialize = function () {
        registerEvents();
    };

    function registerEvents() {
        $('.slider-vertical-up, .slider-product .slider-button-left').on('click', function (e) {
            e.preventDefault();
            previousSlider();
        });

        $('.slider-vertical-down, .slider-button-right').on('click', function (e) {
            e.preventDefault();
            nextSlider();
        });

        $('.navigate li').on('click', function (e) {
            e.preventDefault();
            moveTag($(this));
        })
    }

    function moveTag(el) {
        $('.navigate li.active').removeClass('active');
        el.parent().children().eq(el.index()).addClass('active');
        $('.content li.display').removeClass('display');
        $('.content ul').children().eq(el.index()).addClass('display');
       
    }

    function previousSlider() {
        var lstImages = $('.slider-vertical .lst-img');
        var firstImage = $('.slider-vertical .lst-img li').first();
        var secondImage = $('.slider-vertical .lst-img li').first().next();
        var imgReplace = secondImage.children('img').first().attr('src') === undefined ? firstImage.children('img').first().attr('src') : secondImage.children('img').first().attr('src');
        $('.img-sliders img').first().removeAttr('src').attr({ 'src': imgReplace });
        firstImage.hide('slow', function () {
            firstImage.show();
            lstImages.append(firstImage);
        });
    }

    function nextSlider() {
        var lstImages = $('.slider-vertical .lst-img');
        var lastImage = $('.slider-vertical .lst-img li').last();
        $('.img-sliders img').first().removeAttr('src').attr({ 'src': lastImage.children('img').first().attr('src') });
        lastImage.hide('slow', function () {
            lastImage.show();
            lstImages.prepend(lastImage);
        });
    }
   
};


var productObj = new ProductDetailController();
productObj.initialize();
 