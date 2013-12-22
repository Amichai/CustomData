
function roundMinutes(date) {

    date.setHours(date.getHours() + Math.round(date.getMinutes() / 60));
    date.setMinutes(0);

    return date;
}

function chart2(result) {
    var data = new Array();
    //data.push({ 'letter': 'a', 'frequency': .1 });
    //data.push({ 'letter': 'b', 'frequency': .2 });
    //data.push({ 'letter': 'c', 'frequency': .5 });
    //data.push({ 'letter': 'd', 'frequency': .3 });
    //data.push({ 'letter': 'e', 'frequency': .6 });
    //data.push({ 'letter': 'f', 'frequency': .2 });
    //data.push({ 'letter': 'g', 'frequency': .1 });
    
    for (var r in result) {
        for (var h in result[r].Times) {
            var d = new Date(result[r].Times[h]);
            var key = roundMinutes(d).toDateString();
            if (key in data) {
                data[key]++;
            } else {
                data[key] = 1;
            }
        }
    }

    var margin = { top: 20, right: 20, bottom: 30, left: 40 },
    width = 960 - margin.left - margin.right,
    height = 500 - margin.top - margin.bottom;

    var x = d3.scale.ordinal()
        .rangeRoundBands([0, width], .1);

    var y = d3.scale.linear()
        .range([height, 0]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .orient("bottom");

    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left")
        .ticks(10, "%");

    var svg = d3.select("#chart").append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
      .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    //for (var i = 0; i < Object.keys(data).length ; i++) {
    //    var key = Object.keys(data)[i];
    //    var toAdd = new Object();
    //    toAdd.letter = key;
    //    toAdd.frequency = data[key];
    //    data.push(toAdd)
    //}

    //d3.tsv("data.tsv", type, function (error, data) {

    //x.domain(Object.keys(data).map(function (d) {
    //    debugger;
    //    return data[d].letter;
    //}));
    x.domain(Object.keys(data));
        //x.domain(data.map(function (d) { return d.letter; }));
    y.domain([0, d3.max(Object.keys(data), function (d) {
        return data[d];
    })]);

    debugger;
        svg.append("g")
            .attr("class", "x axis")
            .attr("transform", "translate(0," + height + ")")
            .call(xAxis);

        svg.append("g")
            .attr("class", "y axis")
            .call(yAxis)
          .append("text")
            .attr("transform", "rotate(-90)")
            .attr("y", 6)
            .attr("dy", ".71em")
            .style("text-anchor", "end")
            .text("Frequency");

        //svg.selectAll(".bar")
        //    .data(data)
        //  .enter().append("rect")
        //    .attr("class", "bar")
        //    .attr("x", function (d) { return x(d.letter); })
        //    .attr("width", x.rangeBand())
        //    .attr("y", function (d) { return y(d.frequency); })
        //    .attr("height", function (d) { return height - y(d.frequency); });

        svg.selectAll(".bar")
                .data(Object.keys(data))
              .enter().append("rect")
                .attr("class", "bar")
                .attr("x", function (d) { return x(d); })
                .attr("width", x.rangeBand())
                .attr("y", function (d) { return y(data[d]); })
                .attr("height", function (d) { return height - y(data[d]); });

    //});

    function type(d) {
        d.frequency = +d.frequency;
        return d;
    }

}

function chart1(result) {
    var width = 960,
    height = 136,
    cellSize = 17; // cell size

    var day = d3.time.format("%w"),
        week = d3.time.format("%U"),
        percent = d3.format(".1%"),
        format = d3.time.format("%Y-%m-%d");

    var color = d3.scale.quantize()
        .domain([-.05, .05])
        .range(d3.range(11).map(function (d) { return "q" + d + "-11"; }));

    var svg = d3.select("#chart").selectAll("svg")
        .data(d3.range(2012, 2013))
      .enter().append("svg")
        .attr("width", width)
        .attr("height", height)
        .attr("class", "RdYlGn")
      .append("g")
        .attr("transform", "translate(" + ((width - cellSize * 53) / 2) + "," + (height - cellSize * 7 - 1) + ")");

    svg.append("text")
        .attr("transform", "translate(-6," + cellSize * 3.5 + ")rotate(-90)")
        .style("text-anchor", "middle")
        .text(function (d) { return d; });
    var rect = svg.selectAll(".day")
        .data(function (d) { return d3.time.days(new Date(d, 0, 1), new Date(d + 1, 0, 1)); })
      .enter().append("rect")
        .attr("class", "day")
        .attr("width", cellSize)
        .attr("height", cellSize)
        .attr("x", function (d) { return week(d) * cellSize; })
        .attr("y", function (d) { return day(d) * cellSize; })
        .datum(format);

    rect.append("title")
        .text(function (d) { return d; });

    svg.selectAll(".month")
        .data(function (d) { return d3.time.months(new Date(d, 0, 1), new Date(d + 1, 0, 1)); })
      .enter().append("path")
        .attr("class", "month")
        .attr("d", monthPath);

    rect.map(function (d) {
        d.map(function (day) {
            var r = (Math.random() - .5) / 5;
            $(day).attr("class", function (d) { return "day " + color(r); });
            var oldTitle = $(day).find("title").text();
            $(day).find("title").text(oldTitle + ": " + percent(r));
            return day;
        });
        return d;
    });
    //d3.csv("dji.csv", function (error, csv) {
    //    var data = d3.nest()
    //      .key(function (d) { return d.Date; })
    //      .rollup(function (d) { return (d[0].Close - d[0].Open) / d[0].Open; })
    //      .map(csv);

    //    rect.filter(function (d) { return d in data; })
    //        .attr("class", function (d) { return "day " + color(data[d]); })
    //      .select("title")
    //        .text(function (d) { return d + ": " + percent(data[d]); });
    //});

    function monthPath(t0) {
        var t1 = new Date(t0.getFullYear(), t0.getMonth() + 1, 0),
            d0 = +day(t0), w0 = +week(t0),
            d1 = +day(t1), w1 = +week(t1);
        return "M" + (w0 + 1) * cellSize + "," + d0 * cellSize
            + "H" + w0 * cellSize + "V" + 7 * cellSize
            + "H" + w1 * cellSize + "V" + (d1 + 1) * cellSize
            + "H" + (w1 + 1) * cellSize + "V" + 0
            + "H" + (w0 + 1) * cellSize + "Z";
    }

    d3.select(self.frameElement).style("height", "2910px");
}