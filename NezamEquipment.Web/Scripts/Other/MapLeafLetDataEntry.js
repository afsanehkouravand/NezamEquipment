$("#map").css("width", $(window).width()).css("height", $(window).height());

proj4.defs("EPSG:" + projcode, projdescription);

var OpenStreet = L.tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '',
    id: 'openstreetmap'
});

var Abadan = L.tileLayer.wms(tileLayerLink, {
    maxZoom: 20,
    layers: tileLayerName,
    format: 'image/jpeg',
    version: '1.1.0'
});

//var Abadan = L.tileLayer.wms("http://localhost:1212/geoserver/cite/wms", {
//    maxZoom: 20,
//    layers: 'cite:kamal',
//    format: 'image/jpeg',
//    version: '1.1.0',
//});

var mymap = L.map('map', {
    center: [zoomX, zoomY],
    //center: [30.387198, 48.224623],
    zoom: 11
});

var baseLayers = {
    "Abadan": Abadan,
    "OpenStreet": OpenStreet
};

L.control.layers(baseLayers).addTo(mymap);

mymap.on('baselayerchange', function (e) {
    Cookies.set('currentMapLayer', e.name);
});

if (typeof Cookies.get('currentMapLayer') == 'undefined') {
    Cookies.set('currentMapLayer', "OpenStreet");
    mymap.addLayer(OpenStreet);
} else {
    mymap.addLayer(window[Cookies.get('currentMapLayer')]);
}

L.easyButton('fa-map', function () {
    if (typeof Cookies.get('removelayer') == 'undefined') {
        Cookies.set('removelayer', "1");
    } else {
        Cookies.remove('removelayer');
    }
    ModifyBackgroundLayer();
}).addTo(mymap);

L.easyButton('fa-map-marker', function () {
    if (typeof Cookies.get('gpslocation') == 'undefined') {
        Cookies.set('gpslocation', "1");
    } else {
        Cookies.remove('gpslocation');
    }
    GpsLocation();
}).addTo(mymap);

L.easyButton({
    states: [{
        stateName: 'hide-shape-file',
        icon: 'fa-eye',
        title: 'حذف شیب ها',
        onClick: function (btn, map) {
            allgeojson.forEach(function (e) {
                mymap.removeLayer(e.data);
            });
            btn.state('show-shape-file');
        }
    }, {
        stateName: 'show-shape-file',
        icon: 'fa-eye-slash',
        title: 'نمایش شیب ها',
        onClick: function (btn, map) {
            allgeojson.forEach(function (e) {
                mymap.addLayer(e.data);
            });
            btn.state('hide-shape-file');
        }
    }]
}).addTo(mymap);

var allgeojson = [];

geoJsonData.forEach(function (mapData) {
    $.ajax({
        dataType: "json",
        url: "../GetData/mapbya20anda19?a20str=" + a20str + "&a19str=" + a19str + "&mapid=" + mapData.mapId,
        success: function (gjson) {

            var data =
                L.Proj.geoJson(gjson,
                    {
                        style: function(feature, mapId = mapData.mapId) {
                            return makeStyle(feature, mapId);
                        },
                        onEachFeature: function(feature, layer) {
                            layer.on({
                                mouseover: function(e) {
                                    var layer = e.target;
                                    layer.setStyle({
                                        weight: 5,
                                        color: '#666',
                                        dashArray: '',
                                        fillOpacity: 0.7
                                    });
                                    settingAttrData(layer.feature.properties);
                                },
                                mouseout: function(e, mapId = mapData.mapId) {
                                    allgeojson.forEach(function(agj) {
                                        if (agj.mapid === mapId) {
                                            agj.data.resetStyle(e.target);
                                        }
                                    });
                                },
                                click: onMapClick
                            });
                        }
                    }).addTo(mymap);

            allgeojson.push({
                mapid: mapData.mapId,
                data: data,
            });

            // zoom to layer
            mymap.fitBounds(data.getBounds());
        }
    });
});

function styleGetColor(feature, sData) {
    var fill = sData.defaultStyle.DefaultStyleFillColor;
    var border = sData.defaultStyle.DefaultStyleOutlineColor;
    var borderWidth = sData.defaultStyle.DefaultStyleOutlineWidth;

    if (sData.styles.length > 0) {
        for (var i = 0; i < sData.styles.length; i++) {
            var d = sData.styles[i];
            if (feature.properties['A20'] === d.A20Value && feature.properties['A19'] === d.A19Value && feature.properties['A2'] === d.A2Value) {
                fill = d.FillColor;
                border = d.OutlineColor;
                borderWidth = d.OutlineWidth;
            }
        }
    }

    if (typeof customStyleData != 'undefined') {
        for (var i = 0; i < customStyleData.length; i++) {
            var d = customStyleData[i];
            if (feature.properties['A20'] == d.A20 && feature.properties['A19'] == d.A19 && feature.properties['A2'] == d.A2) {
                fill = '#000000';
                border = '#000000';
                borderWidth = 1;
            }
        }
    }

    return {
        fill: fill,
        border: border,
        borderWidth: borderWidth
    };
}

function styleTemplateExist(mapId) {
    for (var i = 0; i < geoJsonData.length; i++) {
        if (geoJsonData[i].mapId === mapId) {
            return geoJsonData[i];
        }
    }

    return undefined;
}

function makeStyle(feature, mapId) {
    var s = styleTemplateExist(mapId);
    if (typeof s == 'undefined') {
        return null;
    }
    var data = styleGetColor(feature, s);
    return {
        weight: data.borderWidth,
        opacity: 1,
        color: data.border,
        fillOpacity: 0.7,
        fillColor: data.fill
    };
}

ModifyBackgroundLayer();
function ModifyBackgroundLayer() {
    if (Cookies.get('removelayer') === "1") {
        mymap.removeLayer(window[Cookies.get('currentMapLayer')]);
    } else {
        mymap.addLayer(window[Cookies.get('currentMapLayer')]);
    }
}

GpsLocation();
function GpsLocation() {
    if (Cookies.get('gpslocation') === "1") {
        setInterval(function () {
            GetGeoLocation();
        }, 5000);
        $(".btnGpsLocation").text("عدم نمایش مختصات Gps");
    } else {
        if (typeof marker != 'undefined') {
            mymap.removeLayer(marker);
            marker = undefined;
        };
        $(".btnGpsLocation").text("نمایش مختصات Gps");
    }
}
function GetGeoLocation() {
    if (typeof Cookies.get('gpslocation') == 'undefined') {
        return;
    }

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        alert("امکان نمایش مختصات Gps وجود ندارد.");
    }
}
var marker;
function showPosition(position) {
    if (typeof marker != 'undefined') {
        mymap.removeLayer(marker);
    };
    marker = L.marker([position.coords.latitude, position.coords.longitude]).addTo(mymap);
    //marker = L.marker([30.387198, 48.224623]).addTo(mymap);
}

$.ajax({
    dataType: "json",
    url: "../GetData/UiD",
    success: function (data) {
        addUiD(data);
    }
});

//shp('../Upload/UID.zip').then(function (data) {
//    addUiD(data);
//});

function addUiD(data) {
    L.Proj.geoJson(data, {
        style: function () {
            return {
                weight: 1,
                opacity: 1,
                color: 'red',
                fillOpacity: 0
            }
        },
        onEachFeature: function (feature, layer) {
            layer.on({
                mouseover: function () {
                    uniqueCodeletter = feature.properties.First_Item;
                    uniqueCodeBaseNumber = feature.properties.Uniqe_cod;
                }
            });
            layer.bindPopup("<span style=\"display:block;font-size:23px;text-align:center;font-weight:bold;\">" + feature.properties.Uniqe_cod + feature.properties.First_Item + "</span>");
        }
    }).addTo(mymap).bringToBack();
}