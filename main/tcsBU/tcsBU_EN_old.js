/* tcsBU 1.1 (nightly) (c)  amsantos@fc.up.pt */
"use strict";
$(function () {
    /*
     * the svg variable (holds the <SVG> element in DOM)
     */

    var svg = null;

    /*
     * the 'force-directed layout', 'pie' and 'arc' variables for d3.js
     */

    var force = null;

    /*
     * holder for SVG definitions
     */

    var defs = null;

    /*
     * variables that hold nodes and edges read from a file
     */

    var nodeList = [], edgeList = [], linkList = [];

    /*
     * variables that hold nodes which should be highlighted or labeled
     */
    var labelNode = {}, highlightNode = [], seqHapFlag = false, seqHapAdded = false, distanceFlag = false;


    /*
     * variables that determine if clicks delete nodes/links
     */

    var deletelink = false, deletenode = false, outlinenodes = false;

    /*
     * Define some default values for 'force-directed layout' algorithm
     */

    var defaultDistance = 12, defaultGravity = 0.05, defaultCharge = -30, defaultLinkDistance = 1,
        defaultLinkStrength = 1, defaultFriction = 0.95;

    /*
     * standard radius for true haplogroups with 'frequency' = 1
     */

    var standardRadius = 5;


    /*
     * Variables for the forced layout algorithm elements
     */
    var minTime = Number.POSITIVE_INFINITY;
    var maxTime = Number.NEGATIVE_INFINITY;


    var link, node, path, linkText;

    var pie, sector, sectorOuter, sectorNull, sectorSmall;

    var drag;

    var clickLink, clickNode;

    /*
     * Default line width
     */

    var linewidth = 1;

    var typeid = 0;

    var styleid = 0;
    /*
     * Default zoom
     */

    var zoom = null;

    /*
     * Legend 0 -off, 1 - on
     */

    var legend = 0;

    /*
     * Save File available?
     */

    var filesave = false;

    /*
     * Pattern names. See definitions in 'createPattern.js'
     */

    var pattern_names = [{id: 'none'}, {icon: 'icon-circles-1', id: 'circles-1'}, {
        icon: 'icon-circles-2', id: 'circles-2'
    }, {icon: 'icon-lines-1', id: 'lines-1'}, {icon: 'icon-lines-2', id: 'lines-2'}, {
        icon: 'icon-lines-3', id: 'lines-3'
    }, {icon: 'icon-lines-4', id: 'lines-4'}, {icon: 'icon-lines-5', id: 'lines-5'}, {
        icon: 'icon-lines-6', id: 'lines-6'
    }, {icon: 'icon-lines-7', id: 'lines-7'}, {icon: 'icon-lines-8', id: 'lines-8'}, {
        icon: 'icon-cross-1', id: 'cross-1'
    }, {icon: 'icon-cross-2', id: 'cross-2'}, {icon: 'icon-cross-3', id: 'cross-3'}, {
        icon: 'icon-cross-4', id: 'cross-4'
    }];


    /*
     * This function reclassifies (changes the group and color) a given haplotype
     * in the haplotype list (referred by its 'index') and changes the svg graph
     * (if enabled) accordingly. The new and old values of the group should
     * be passed, to update the proportions of the corresponding pie chart
     */

    function classify(index, newgroup, oldgroup) {

        /*
         * Grab the new color from groups' list ('newgroup' is yhe index of that array)
         * Additionally, grab a pattern if available
         */
        var newcolor = 'ffffff', newpattern = 'none';

        var nc = w2ui.groups.find({recid: newgroup}, true)[0];
        if (nc !== 'undefined') {
            newcolor = w2ui.groups.records[nc].color;
            if (w2ui.groups.records[nc].pattern !== 'none') newpattern = "url(#" + newcolor + w2ui.groups.records[nc].pattern + ")";
        }

        /*
         * Grab the haplogroup of this particular haploty++pe
         */

        var haplogroup = Number(w2ui.haplotypes.records[index].haplogroup);
        var haplocount = Number(w2ui.haplotypes.records[index].count);
        var haplotimecolor = w2ui.haplotypes.records[index].timecolor;
        var haplotime = w2ui.haplotypes.records[index].time;
        var haplostyle = w2ui.haplotypes.records[index].nodestyle;
        /*
         * Grab the node index from nodeList where id == haplogroup
         */

        var nd = $.map(nodeList, function (e, i) {
            if (e.id === haplogroup) return i;
        })[0];

        if (typeof nd !== 'undefined') {

            /*
             * If a node was found, check if the group of this haplotype is already
             * present in the 'proportions' property of the respective node (ng).
             * Check also the index of the group being changed (og): this is usually
             * the default group, but may be any other groups that's being changed.
             */

            var ng = $.map(nodeList[nd].proportions, function (e, i) {
                if (e.group === newgroup) return i;
            })[0];
            var og = $.map(nodeList[nd].proportions, function (e, i) {
                if (e.group === oldgroup) return i;
            })[0];

            /*
             * If ng is undefined, it means that the haplogroup has no elements classified
             * as 'newgroup'. Add this new group to 'proportions' property, with a value
             * of 1, and remove one element from the value of the old group. If both ng and
             * og are defined, increment the proportions' value of ng and decrement the
             * value of og.
             */

            if (typeof og !== 'undefined') {
                if (typeof ng !== 'undefined') {
                    nodeList[nd].proportions[ng].value += haplocount;
                    nodeList[nd].proportions[ng].color = '#' + newcolor;
                    nodeList[nd].proportions[ng].pattern = newpattern;
                    nodeList[nd].proportions[ng].timecolor = haplotimecolor;
                    nodeList[nd].proportions[ng].time = haplotime;
                    nodeList[nd].proportions[ng].nodestyle = haplostyle;
                    nodeList[nd].proportions[og].value -= haplocount;
                } else {
                    nodeList[nd].proportions.push({
                        color: '#' + newcolor,
                        group: newgroup,
                        radius: nodeList[nd].radius,
                        value: haplocount,
                        pattern: newpattern,
                        timecolor: haplotimecolor,
                        nodestyle: haplostyle,
                        time: haplotime
                    });
                    nodeList[nd].proportions[og].value -= haplocount;
                }

                /*
                 * Find the target svg element (node). If it exists, apply changes
                 */

                if (svg) {
                    var n = svg.selectAll('.node')
                    path = n.selectAll('path').data(function (d) {
                        return pie(d.proportions);
                    });


                    path.enter()
                        .append('path')
                        .attr('d', function (d) {
                            if (isINNerNode(d)) {
                                return sector(d)
                            } else {
                                if (styleid === 1) return sector(d); else if (styleid === 2) return sectorNull(d); else return sectorOuter(d);
                            }
                        })
                        .style('fill', function (d) {
                            if (d.data.pattern === 'none') return d.data.color; else return d.data.pattern;
                        });
                    if (outlinenodes) {
                        path.style('stroke-width', linewidth / 2).style('stroke', '#000000');
                    } else {
                        path.style('stroke-width', '0').style('stroke', 'none');
                    }


                    var colorGradientScale = d3.scale.linear()
                        .domain([minTime, maxTime])
                        .range(["#cbc6d8", "#601824"]);

                    path.enter()
                        .append('path')
                        .attr('d', function (d) {
                            if (isINNerNode(d)) {
                                return sectorNull(d);
                            } else {
                                if (styleid === 1) return sectorNull(d); else if (styleid === 2) return sector(d); else return sectorSmall(d);
                            }
                        })
                        .style('fill', function (d) {
                            if (d.data.pattern === 'none') {
                                if (typeid === 0) return '#' + d.data.timecolor + d.data.timecolor + d.data.timecolor;
                                if (typeid === 1) return '#ff' + d.data.timecolor + d.data.timecolor;
                                if (typeid === 2) return '#' + d.data.timecolor + 'ff' + d.data.timecolor;
                                if (typeid === 3) return '#' + d.data.timecolor + d.data.timecolor + 'ff';
                                if (typeid === 4) return '#ffffff';
                                if (typeid === 5) return colorGradientScale(d.data.time);

                            } else return d.data.pattern;
                        });


                    /*
                     * These two commands may be used to implement
                     * a stroke between arcs in the pie
                     * .style('stroke-width', '0')
                     * .style('stroke', 'none');
                     */
                    if (outlinenodes) {
                        path.style('stroke-width', linewidth / 2).style('stroke', '#000000');
                    } else {
                        path.style('stroke-width', '0').style('stroke', 'none');
                    }
                    path.exit().remove();

                    force.nodes(nodeList).links(linkList).start();
                }

            } else {
                w2alert('Serious error!', 'ERROR');
            }
        }
    }

    /*
     * Create an SVG pattern definition to be used as a URL
     * 'idx' is the
     */

    function createPattern(pat, col) {
        if (pat === 'none') return;
        var name = col + pat;
        var p = defs.append("pattern");
        p.attr("id", name)
            .attr("patternUnits", "userSpaceOnUse")
            .attr("width", 10)
            .attr("height", 10);

        /*
         * Append a rect that will hold the background color
         */

        p.append("rect")
            .attr("width", "10")
            .attr("height", "10")
            .attr("x", "0")
            .attr("y", "0")
            .attr("fill", "#" + col)
            .attr("stroke-width", "0");

        switch (pat) {
            case 'lines-1':
                p.append('path')
                    .attr('d', 'M3,0 V10 M8,0 V10')
                    .attr('stroke', '#000000')
                    .attr('stroke-width', 0.5);
                break;
            case 'lines-2':
                p.append('path')
                    .attr('d', 'M3,0 V10 M8,0 V10')
                    .attr('stroke', '#000000')
                    .attr('stroke-width', 1.5);
                break;
            case 'lines-3':
                p.append('path')
                    .attr('d', 'M0,3 H10 M0,8 H10')
                    .attr('stroke', '#000000')
                    .attr('stroke-width', 0.5);
                break;
            case 'lines-4':
                p.append('path')
                    .attr('d', 'M0,3 H10 M0,8 H10')
                    .attr('stroke', '#000000')
                    .attr('stroke-width', 1.5);
                break;
            case 'lines-5':
                p.append('path')
                    .attr('d', 'M4,-1 l10,10 M-1,4 l10,10')
                    .attr('stroke', '#000000')
                    .attr('stroke-width', 0.5);
                break;
            case 'lines-6':
                p.append('path')
                    .attr('d', 'M4,-1 l10,10 M-1,4 l10,10')
                    .attr('stroke', '#000000')
                    .attr('stroke-width', 1.5);
                break;
            case 'lines-7':
                p.append('path')
                    .attr('d', 'M4,-1 l-10,10 M2,11 l10,-10')
                    .attr('stroke', '#000000')
                    .attr('stroke-width', 0.5);
                break;
            case 'lines-8':
                p.append('path')
                    .attr('d', 'M4,-1 l-10,10 M2,11 l10,-10')
                    .attr('stroke', '#000000')
                    .attr('stroke-width', 1.5);
                break;
            case 'cross-1':
                p.append('path')
                    .attr('d', 'M3,0 V10 M8,0 V10 M0,3 H10 M0,8 H10')
                    .attr('stroke', '#000000')
                    .attr('stroke-width', 0.5);
                break;
            case 'cross-2':
                p.append('path')
                    .attr('d', 'M3,0 V10 M8,0 V10 M0,3 H10 M0,8 H10')
                    .attr('stroke', '#000000')
                    .attr('stroke-width', 1.5);
                break;
            case 'cross-3':
                p.append('path')
                    .attr('d', 'M0,0 L10,10 M10,0 L0,10')
                    .attr('stroke', '#000000')
                    .attr('stroke-width', 0.5);
                break;
            case 'cross-4':
                p.append('path')
                    .attr('d', 'M0,0 L10,10 M10,0 L0,10')
                    .attr('stroke', '#000000')
                    .attr('stroke-width', 1.5);
                break;
            case 'circles-1':
                p.append("rect").attr({width: "10", height: "10", fill: "#" + col});
                p.append("circle")
                    .attr({cx: 2, cy: 2, r: 1, transform: "translate(0,0)", fill: "#000000"});
                p.append("circle")
                    .attr({cx: 2, cy: 7, r: 1, transform: "translate(0,0)", fill: "#000000"});
                p.append("circle")
                    .attr({cx: 7, cy: 2, r: 1, transform: "translate(0,0)", fill: "#000000"});
                p.append("circle")
                    .attr({cx: 7, cy: 7, r: 1, transform: "translate(0,0)", fill: "#000000"});
                break;
            case 'circles-2':
                p.append("rect").attr({width: "10", height: "10", fill: "#" + col});
                p.append("circle")
                    .attr({cx: 5, cy: 5, r: 3, transform: "translate(0,0)", fill: "#000000"})
                break;
        }
    }

    function isINNerNode(d) {
        return d.data.nodestyle === 0;
    }

    function updateSVG() {
        link = svg.selectAll('.link').remove();
        link = svg.selectAll('.link').data(linkList);
        link.enter().append('line')
            .attr('class', 'link')
            .attr('id', function (d) {
                return d.id;
            })
            .on('mouseover', function () {
                d3.select(this).style({'stroke': '#FF011B', 'stroke-width': linewidth * 3});
            })
            .on('mouseout', function () {
                d3.select(this).style({'stroke': '#000000', 'stroke-width': linewidth});
            })
            .on('click', clickLink)
            .append('title')
            .text(function (d) {
                return d.changes;
            });
        link.style('stroke-width', linewidth).style('stroke', '#000000');
        link.exit().remove();

        node = svg.selectAll('.node').remove();
        node = svg.selectAll('.node').data(nodeList);
        node.enter().append('g')
            .attr('class', 'node')
            .attr('id', function (d) {
                return d.name;
            })
            .on('click', clickNode)
            .call(drag)
            .append('circle')
            .attr('class', 'node-circle')
            .attr('r', function (d) {
                if (d.nodestyle === 1) {
                    if (styleid === 1 || styleid === 2) return d.radius; else return d.radius * 1.4;
                } else {
                    return d.radius;
                }
            })
            // .on('mouseover', function () { d3.select(this).style({'stroke': '#FF011B', 'stroke-width': linewidth * 4}); })
            // .on('mouseout', function () { d3.select(this).style({'stroke': '#000000', 'stroke-width': linewidth * 2}); })
            .append('title')
            .text(function (d) {
                return d.name;
            });

        node.style('stroke-width', function () {
            return linewidth * 2;
        }).style('stroke', '#000000').style('fill', '#000000');
        node.exit().remove();


        path = node.selectAll('path').remove();
        path = node.selectAll('path').data(function (d) {
            return pie(d.proportions);
        });

        path.enter()
            .append('path')
            .attr('d', function (d) {
                if (isINNerNode(d)) {
                    return sector(d)
                } else {
                    if (styleid === 1) return sector(d); else if (styleid === 2) return sectorNull(d); else return sectorOuter(d);
                }
            })
            .style('fill', function (d) {
                if (d.data.pattern === 'none') return d.data.color; else return d.data.pattern;
            });
        if (outlinenodes) {
            path.style('stroke-width', linewidth / 2).style('stroke', '#000000');
        } else {
            path.style('stroke-width', '0').style('stroke', 'none');
        }

        var colorGradientScale = d3.scale.linear()
            .domain([minTime, maxTime])
            .range(["#cbc6d8", "#601824"]);

        path.enter()
            .append('path')
            .attr('d', function (d) {
                if (isINNerNode(d)) {
                    return sectorNull(d);
                } else {
                    if (styleid === 1) return sectorNull(d); else if (styleid === 2) return sector(d); else return sectorSmall(d);
                }
            })
            .style('fill', function (d) {
                if (d.data.pattern === 'none') {
                    if (typeid === 0) return '#' + d.data.timecolor + d.data.timecolor + d.data.timecolor;
                    if (typeid === 1) return '#ff' + d.data.timecolor + d.data.timecolor;
                    if (typeid === 2) return '#' + d.data.timecolor + 'ff' + d.data.timecolor;
                    if (typeid === 3) return '#' + d.data.timecolor + d.data.timecolor + 'ff';
                    if (typeid === 4) return '#ffffff';
                    if (typeid === 5) return colorGradientScale(d.data.time);
                } else return d.data.pattern;
            });


        /*
         * These two commands may be used to implement
         * a stroke between arcs in the pie
         * .style('stroke-width', '0')
         * .style('stroke', 'none');
         */
        if (outlinenodes) {
            path.style('stroke-width', linewidth / 2).style('stroke', '#000000');
        } else {
            path.style('stroke-width', '0').style('stroke', 'none');
        }
        path.exit().remove();

        linkText = svg.selectAll(".link-text").remove();
        if (distanceFlag) {
            linkText = svg.selectAll('.link-text').data(linkList);
            linkText.enter().append('text')
                .attr('class', 'link-text')
                .attr('text-anchor', 'middle')
                .text(function (d) {
                    return d.changes;
                })
                .style('font-family', 'Times New Roman') // Set the font family
                .style("stroke-width", '0.2px')
                .style('font-size', '13px');
            linkText.exit().remove();
        }

        force.nodes(nodeList).links(linkList).start();

        // 根据highlightnode和labelnode两个全局变量 对svg进行修饰
        highlightNode.forEach(function (name) {
            // 构造选择器 对name中的换行符进行转义
            var selector = '#' + name.replaceAll("\n", "\\a ");
            d3.select(selector).select(".node-circle").style({'stroke': '#FF0000', 'stroke-width': linewidth * 4});
        });

        Object.entries(labelNode).forEach(([key, value]) => {
            // 构造选择器 对name中的换行符进行转义
            var selector = '#' + key.replaceAll("\n", "\\a ");
            d3.select(selector)
                .append('text')
                .attr('class', 'node-text')
                .attr('dx', function (d) {
                    if (d.nodestyle === 1) {
                        if (styleid === 1 || styleid === 2) return 5 + d.radius; else return 5 + d.radius * 1.4;
                    } else {
                        return 5 + d.radius;
                    }
                })
                .attr('dy', '.35em')
                .text(value.join(";"))
                .style('font-family', 'Times New Roman') // Set the font family
                .style("stroke-width", '0.2px')
                .style('font-size', '13px');
        });

        // show the haplotype of every node on the svg
        if (seqHapFlag) {
            d3.selectAll('.node')
                .each(function (d) {
                    d3.select(this)
                        .append('text')
                        .attr('class', 'node_hap')
                        .attr('dx', function (d) {
                            if (d.nodestyle === 1) {
                                if (styleid === 1 || styleid === 2) {
                                    return 5 + d.radius;
                                } else {
                                    return 5 + d.radius * 1.4;
                                }
                            } else {
                                return 5 + d.radius;
                            }
                        })
                        .attr('dy', '.50em')
                        .text(d.seq2hap)
                        .style('font-family', 'Times New Roman') // Set the font family
                        .style("stroke-width", '0.2px')
                        .style('font-size', '13px');
                });
        }
    }

    /*
     * The group's grid
     */

    function getGroupsGrid() {

        /*
         * Append an <input> element at the end of the body. This will serve as an anchor to be used
         * by button 'loadGroups' to browse files for reading a group's file (csv delimited text
         * file with two columns: group names and rgb colors
         */

        $('body').append('<input id="loadGroups" type="file" />');

        /*
         * After selecting a file, this triggers loadGroups function
         */

        $('#loadGroups').on('change', function (e) {
            loadGroups(e);
        });

        /*
         * Clear the input field on each 'click', thus allowing to read the
         * same file after it is modified externally. Otherwise, the file
         * won't load a second time it is opned!
         */

        $('#loadGroups').on('click', function () {
            this.value = null;
        });

        /*
         * Use w2ui to build the group's grid
         */

        $().w2grid({
            name: 'groups', multiSelect: false, show: {
                header: false,
                toolbar: true,
                footer: true,
                lineNumbers: false,
                toolbarSearch: false,
                toolbarInput: false,
                toolbarReload: false,
                toolbarColumns: false,
                toolbarSave: false,
                toolbarAdd: false,
                toolbarDelete: false,
                toolbarEdit: false
            }, columns: [{
                field: 'recid',
                caption: 'Group',
                size: '45%',
                sortable: true,
                resizable: false,
                editable: {type: 'text'},
                render: function (r) {
                    return '<div style="font-weight: bold">' + r.recid + '</div>';
                }
            }, {
                field: 'color',
                caption: 'Color',
                size: '20%',
                sortable: false,
                resizable: false,
                editable: {type: 'color'},
                render: function (r) {
                    return '<div style="background-color: #' + r.color + '">&nbsp;</div>';
                }
            }, {
                field: 'pattern',
                caption: 'Pattern',
                size: '35%',
                sortable: false,
                resizable: false,
                editable: {type: 'combo', items: pattern_names, filter: false},
                render: function (r) {
                    return '<div>' + r.pattern + '</div>';
                }
            }], records: [{recid: 'Default', color: 'ffffff', pattern: 'none', editable: false}], toolbar: {
                items: [{type: 'button', id: 'add_group', caption: 'Add', icon: 'w2ui-icon-plus'}, {
                    type: 'button', id: 'del_group', caption: 'Delete', icon: 'w2ui-icon-cross'
                }, {type: 'button', id: 'load_group', caption: 'Load', icon: 'icon-folder-open'}, {
                    type: 'button', id: 'save_group', caption: 'Save', icon: 'icon-file-save'
                }], onClick: function (e) {
                    switch (e.target) {
                        case 'add_group':
                            e.preventDefault();

                            /*
                             * Check if a group with the default name ('New Group') exists! If so get out...
                             */

                            var v = w2ui.groups.find({recid: 'New Group'}, true);
                            if (v.length > 0) w2alert('A group named "New Group" already exists! <p> Change its name first...'); else {
                                w2ui.groups.add({recid: 'New Group', color: 'ffffff', pattern: 'none'});
                                this.refresh();
                            }
                            break;
                        case 'del_group':
                            e.force = true;
                            var sel = w2ui.groups.getSelection()[0];


                            /*
                             * Check if selected group is the default one [0]
                             */

                            if (sel === 0) {
                                e.preventDefault();
                                w2alert('Cannot delete "default" group/color...');
                            } else {

                                /*
                                 * Check if 'haplotype' grid is already defined and that no
                                 * record of w2ui.haplotypes references this record from w2ui.groups
                                 */

                                if (w2ui.haplotypes) {
                                    var h = w2ui.haplotypes.records.filter(function (o) {
                                        return o.group === sel;
                                    });
                                    if (h.length > 0) {
                                        e.preventDefault();
                                        w2alert('This group is being referenced in haplotype list!<p>Change it there, and then delete it...');
                                    } else {
                                        w2ui.groups.delete(true);
                                    }
                                } else w2ui.groups.delete(true);
                            }
                            break;
                        case 'load_group':
                            $('#loadGroups').click();
                            break;
                        case 'save_group':
                            saveGroups();
                            break;
                    }

                    if (legend === 1) {
                        $('.legend').remove();
                        legend = 0;
                        insertLegend();
                    }
                }
            }, onChange: function (e) {
                var i;
                e.preventDefault();
                switch (e.column) {
                    case 0:  // Change a name

                        /*
                         * Check if a group with the same name already exists! If so get out...
                         */

                        var v = w2ui.groups.find({recid: e.value_new}, true);
                        if (v.length > 0) {
                            w2alert('A group named "' + e.value_new + '" already exists! <p> Change its name first...');
                        } else {
                            w2ui.groups.records[e.index].recid = e.value_new;

                            /*
                             * If this name is in haplotype list, change it as well
                             */

                            v = w2ui.haplotypes.find({group: e.value_original}, true);
                            if (typeof v !== 'undefined' && v.length > 0) {
                                for (i = 0; i < v.length; i++) {
                                    w2ui.haplotypes.records[v[i]].group = e.value_new;
                                    if (svg) classify(v[i], e.value_new, e.value_original);
                                }
                            }
                        }
                        break;
                    case 1: // Change a color

                        w2ui.groups.records[e.index].color = e.value_new;

                        /*
                         * If there is a pattern, update it
                         */

                        if (w2ui.groups.records[e.index].pattern !== 'none') {
                            $("#" + e.value_original + w2ui.groups.records[e.index].pattern).remove();
                            createPattern(w2ui.groups.records[e.index].pattern, e.value_new);
                        }


                        /*
                         * If this name is in haplotype list, change its color as well
                         */

                        v = w2ui.haplotypes.find({group: e.recid}, true);
                        if (typeof v !== 'undefined' && v.length > 0) {
                            for (i = 0; i < v.length; i++) {
                                w2ui.haplotypes.records[v[i]].color = e.value_new;
                                if (svg) classify(v[i], e.recid, e.recid);
                            }
                        }
                        break;

                    case 2: // Change a pattern

                        w2ui.groups.records[e.index].pattern = e.value_new;

                        /*
                         * Update the SVG
                         */

                        if (svg) {
                            var c = w2ui.groups.records[e.index].color;
                            if (e.value_original !== 'none') $("#" + c + e.value_original).remove();
                            if (e.value_new !== 'none') createPattern(e.value_new, c);

                            /*
                             * Reclassify any haloptypes that belong to this group
                             */

                            v = w2ui.haplotypes.find({group: e.recid}, true);
                            if (typeof v !== 'undefined' && v.length > 0) {
                                for (i = 0; i < v.length; i++) {
                                    classify(v[i], e.recid, e.recid);
                                }
                            }
                        }
                        break;
                }

                /*
                 * If legend is present, delete it and redraw it
                 */

                if (legend === 1) {
                    $('.legend').remove();
                    legend = 0;
                    insertLegend();
                }

                this.refresh();
            }
        });
        if (w2ui.groups) return w2ui.groups; else return null;
    }

    // The haplotype's grid
    function getHaplotypesGrid() {

        /*
         * Append an <input> element at the end of the body. This will serve as an anchor to be used
         * by button 'loadHaplotypes' to browse files for reading a haplotype's file (csv delimited text
         * file with two columns: haplotype names and group names
         */

        $('body').append('<input id="loadHaplotypes" type="file" />');

        /*
         * After selecting a file, this triggers loadHaplotypes function
         */

        $('#loadHaplotypes').on('change', function (e) {
            loadHaplotypes(e);
        });

        /*
         * Clear the input field on each 'click', thus allowing to read the
         * same file after it is modified externally. Otherwise, the file
         * won't load a second time it is opened!
         */

        //$('#loadHaplotypes').on('click', function (){ this.value = null; } );

        $().w2grid({
            name: 'haplotypes', multiSelect: true, show: {
                header: false,
                toolbar: true,
                footer: true,
                lineNumbers: false,
                toolbarSearch: false,
                toolbarInput: false,
                toolbarReload: false,
                toolbarColumns: false,
                toolbarAdd: false,
                toolbarDelete: false,
                toolbarEdit: false
            }, columns: [{
                field: 'recid', caption: 'Id', // size: '40%',
                sortable: true, resizable: true
            }, {field: 'haplogroup', hidden: true}, {field: 'color', hidden: true}, {
                field: 'group',
                caption: 'Group', // size: '24%',
                sortable: true,
                resizable: true,
                editable: {type: 'combo', items: ['Default'], filter: false},
                render: function (r) {
                    var color = parseInt(r.color, 16);
                    var textcolor;
                    if (color > 8388607) textcolor = '#000000'; else textcolor = '#ffffff';
                    return '<div style="background-color: #' + r.color + '; color: ' + textcolor + '">' + r.group + '</div>';
                }
            }, {
                field: 'seq2hap', caption: 'SeqHap', sortable: true, resizable: true
            }, {
                field: 'highlight', caption: 'Highlight', resizable: true, editable: false, // 禁止用户编辑
                render: function (r) {
                    // 显示为复选框但禁用编辑
                    return '<input type="checkbox" ' + (r.highlight ? 'checked' : '') + ' disabled>';
                }
            }, {
                field: 'label', caption: 'Mark', resizable: true, editable: false, // 禁止用户编辑
                render: function (r) {
                    // 显示为复选框但禁用编辑
                    return '<input type="checkbox" ' + (r.label ? 'checked' : '') + ' disabled>';
                }
            }], toolbar: {
                items: [{
                    type: 'button', id: 'load_haplotypes', text: 'Load', icon: 'icon-folder-open'
                }, {type: 'button', id: 'save_haplotypes', text: 'Save', icon: 'icon-file-save'}, /*
                        添加高亮按钮 用于将选择的record在svg图中进行高亮显示
                     */
                    {type: 'button', id: 'highlight_haplotypes', text: 'Highlight', icon: 'icon-highlight'},

                    /*
                        添加text按钮 用于将选择的record的信息显示在svg图中
                     */
                    {type: 'button', id: 'label_haplotype', text: 'Mark', icon: 'icon-label'}
                    ,], onClick: function (e) {
                    switch (e.target) {
                        case 'load_haplotypes':

                            /*
                             * Bail out if there is no haplotype list already defined
                             */

                            if (w2ui.haplotypes.records.length === 0) {
                                w2alert('No data loaded yet. Haplotype/Groups data can  <br>' + 'only be imported when a list of haplotypes has <br>' + 'already been loaded through "Load Data" button.<br>', 'Data not yet loaded!');
                            } else {
                                $('#loadHaplotypes').click();
                            }
                            break;
                        case 'save_haplotypes':
                            saveHaplotypes();
                            break;
                        case 'highlight_haplotypes':
                            highlightHaplotypes();
                            break;
                        case 'label_haplotype':
                            labelHaplotype();
                            break;
                    }
                }
            }, onChange: function (e) {
                e.preventDefault();

                /*
                 * get the new group index of this particular haplotype
                 */

                var r = w2ui.groups.find({recid: e.value_new}, true)[0];

                /*
                 * if groups' name exist just change the name and color in haplotypes.
                 * Otherwise use defaults
                 */

                if (typeof r !== 'undefined') {
                    w2ui.haplotypes.records[e.index].group = w2ui.groups.records[r].recid;
                    w2ui.haplotypes.records[e.index].color = w2ui.groups.records[r].color;
                } else {
                    w2ui.haplotypes.records[e.index].group = 'Default';
                    w2ui.haplotypes.records[e.index].color = 'ffffff';
                }

                classify(e.index, e.value_new, e.value_original);
            }, onEditField: function () {
                var items = [];
                var grplist = w2ui.groups.records;
                for (var i = 0; i < grplist.length; i++) {
                    items.push(grplist[i].recid);
                }
                this.columns[3].editable.items = items;
            }
        });
        if (w2ui.haplotypes) return w2ui.haplotypes; else return null;
    }

    /*
        The characters grid
     */
    function getTraitsGrid() {

        /*
         * Append an <input> element at the end of the body. This will serve as an anchor to be used
         * by button 'loadHaplotypes' to browse files for reading a haplotype's file (csv delimited text
         * file with two columns: haplotype names and group names
         */

        $('body').append('<input id="loadTraits" type="file" />');

        /*
         * After selecting a file, this triggers loadHaplotypes function
         */

        $('#loadTraits').on('change', function (e) {
            loadTraits(e);
        });

        /*
         * Clear the input field on each 'click', thus allowing to read the
         * same file after it is modified externally. Otherwise, the file
         * won't load a second time it is opened!
         */

        // $('#loadHaplotypes').on('click', function (){ this.value = null; } );

        $().w2grid({
            name: 'characters', multiSelect: false, show: {
                header: false,
                toolbar: true,
                footer: true,
                lineNumbers: false,
                toolbarSearch: false,
                toolbarInput: false,
                toolbarReload: false,
                toolbarColumns: false,
                toolbarAdd: false,
                toolbarDelete: false,
                toolbarEdit: false
            }, columns: [{
                field: 'recid', caption: 'Traits', size: '55%', sortable: true, resizable: true
            }, {
                field: 'traitNum', caption: 'Trait Number', size: '45%', sortable: true, resizable: true
            }, {field: 'traitDetail', caption: 'Trait Detail', hidden: true}], toolbar: {
                items: [{
                    type: 'button', id: 'load_characters', text: 'Load', icon: 'icon-folder-open'
                }
                    // , {type: 'button', id: 'save_characters', text: 'Save', icon: 'icon-file-save'}
                ], onClick: function (e) {
                    switch (e.target) {
                        case 'load_characters':
                            $('#loadTraits').click();
                            break;
                        case 'save_characters':
                            // todo: implement the function to save the trait info
                            saveCharacters();
                            break;
                    }
                }
            }, onDblClick: function (event) {
                event.preventDefault();
                var traitID = event.recid;
                if (confirm("Select this trait to display?")) {
                    var newHapconffile = generateHapconffile(w2ui.characters.sampleTraitsInfo[traitID]);
                    var newGroupconffile = generateGroupconffile(w2ui.characters.get(traitID));
                    loadGroups(newGroupconffile);
                    loadHaplotypes(newHapconffile);
                }
            }

        });

        // 如果网格已经创建，返回创建的网格对象
        if (w2ui.characters) return w2ui.characters; else return null;
    }


    function insertLegend() {
        if (legend === 0) {
            legend = 1;
            var grouplist = [];
            var colorlist = [];
            var g = w2ui.groups.records;
            for (var i = 1; i < g.length; i++) {
                grouplist.push(g[i].recid);
                if (g[i].pattern !== 'none') colorlist.push("url(#" + g[i].color + g[i].pattern + ")"); else colorlist.push('#' + g[i].color);
            }
            if (styleid !== 1 && typeid !== 4) {
                grouplist.push(String(minTime) + ' (min)');
                grouplist.push(String(maxTime) + ' (max)');
                if (typeid === 1) {
                    colorlist.push('#fff8f8');
                    colorlist.push('#ff0808');
                }
                if (typeid === 2) {
                    colorlist.push('#f8fff8');
                    colorlist.push('#08ff08');
                }
                if (typeid === 3) {
                    colorlist.push('#f8f8ff');
                    colorlist.push('#0808ff');
                }
                if (typeid === 0) {
                    colorlist.push('#f8f8f8');
                    colorlist.push('#080808');
                }
                if (typeid === 5) {
                    colorlist.push('#1e9600');
                    colorlist.push('#ff0000');
                }

            }
            //var sampleOrdinal = d3.scale.category20().domain(grouplist);

            var lg = d3.scale.ordinal();
            lg.domain(grouplist);
            lg.range(colorlist);
            var verticalLegend = d3.svg.legend().labelFormat('none').cellPadding(5).orientation('vertical').units('Groups').cellWidth(25).cellHeight(18).inputScale(lg).cellStepping(10);

            d3.select('svg').append('g').attr('transform', 'translate(50,140)').attr('class', 'legend').call(verticalLegend);
        } else {
            legend = 0;
            $('.legend').remove();
        }
    }

    /*
     * Construct the layout
     */

    function getLayout(style, groups, haplotypes, characters) {
        $('#layout').w2layout({
            name: 'Layout', padding: 0, panels: [{type: 'top', size: 40, resizable: false, style: style}, {
                type: 'left', size: 350, maxSize: 350, resizable: true, title: 'Data', style: style, tabs: {
                    name: 'tabs',
                    active: 'tab1',
                    tabs: [{id: 'tab3', text: 'Traits'}, {id: 'tab1', text: 'Haplotypes'}, {
                        id: 'tab2', text: 'Groups'
                    },],
                    onClick: function (id) {
                        switch (id.target) {
                            case 'tab1':
                                w2ui.Layout.content('left', haplotypes);
                                break;
                            case 'tab2':
                                w2ui.Layout.content('left', groups);
                                break;
                            case 'tab3':
                                w2ui.Layout.content('left', characters);
                                break;
                        }
                    }

                }
            }, {type: 'right', size: 250, resizable: false, title: 'Settings', style: style}, {
                type: 'main', size: '100%', overflow: 'hidden', style: style, toolbar: {
                    items: [{
                        id: 'btn-svgsave', type: 'button', text: 'Save SVG', icon: 'icon-file-svg', disabled: true
                    }, //{ id: 'btn-pdfsave', type: 'button', text: 'Save PDF', icon: 'icon-file-pdf-o', disabled: true },
                        {type: 'break'}, {
                            id: 'btn-zoomin',
                            class: 'zoom-btn',
                            type: 'button',
                            text: 'Zoom In',
                            icon: 'icon-zoom-in',
                            disabled: true
                        }, {
                            id: 'btn-zoomout',
                            class: 'zoom-btn',
                            type: 'button',
                            text: 'Zoom Out',
                            icon: 'icon-zoom-out',
                            disabled: true
                        }, {type: 'break'}, {
                            id: 'btn-delnode',
                            type: 'check',
                            text: 'Delete Node',
                            icon: 'icon-delete-node',
                            disabled: true,
                            checked: false
                        }, {
                            id: 'btn-dellink',
                            type: 'check',
                            text: 'Delete Link',
                            icon: 'icon-delete-link',
                            disabled: true,
                            checked: false
                        }, {type: 'break'}, {
                            id: 'btn-outline',
                            type: 'check',
                            text: 'Outline',
                            icon: 'icon-outline',
                            disabled: true,
                            checked: false
                        }, {
                            id: 'btn-lwidth',
                            type: 'menu',
                            text: 'Line width',
                            icon: 'icon-line-width',
                            disabled: true,
                            items: [{text: '0.1 px', lwidth: "0.1"}, {text: '0.2 px', lwidth: "0.2"}, {
                                text: '0.4 px', lwidth: "0.4"
                            }, {text: '0.6 px', lwidth: "0.6"}, {text: '0.8 px', lwidth: "0.8"}, {
                                text: '1.0 px', lwidth: "1.0"
                            }, {text: '1.5 px', lwidth: "1.5"}, {text: '2.0 px', lwidth: "2.0"}, {
                                text: '3.0 px', lwidth: "3.0"
                            }, {text: '4.0 px', lwidth: "4.0"}, {text: '5.0 px', lwidth: "5.0"},]
                        }, {
                            id: 'btn-time',
                            type: 'menu',
                            text: 'Circle Center',
                            icon: 'icon-circles-2',
                            disabled: true,
                            items: [{text: 'Gray', typeid: "0"}, {text: 'Red', typeid: "1"}, {
                                text: 'Green', typeid: "2"
                            }, {text: 'Blue', typeid: "3"}, {text: 'Null', typeid: "4"}, {text: 'Color', typeid: "5"}]
                        }, {
                            id: 'btn-style',
                            type: 'menu',
                            text: 'Style',
                            icon: 'icon-cross-4',
                            disabled: true,
                            items: [{text: 'All', styleid: "0"}, {text: 'Trait1', styleid: "1"}, {
                                text: 'Trait2', styleid: "2"
                            },]
                        }, {type: 'break'}, {
                            id: 'btn-legend',
                            type: 'check',
                            text: 'Legend',
                            icon: 'icon-legend',
                            disabled: true,
                            checked: false
                        },
                        // todo: 添加两个功能 分别是显示单倍型类型和单倍型之间的碱基差异
                        {
                            id: 'btn-haplotype',
                            type: 'check',
                            text: 'Haplotype',
                            icon: 'icon-legend',
                            disabled: true,
                            checked: false
                        }, {
                            id: 'btn-distance',
                            type: 'check',
                            text: 'Distance',
                            icon: 'icon-legend',
                            disabled: true,
                            checked: false
                        },], onClick: function (e) {
                        var target = e.target;
                        switch (target) {
                            case 'btn-dellink':
                                deletelink = !e.item.checked;
                                break;
                            case 'btn-delnode':
                                deletenode = !e.item.checked;
                                break;
                            case 'btn-svgsave':
                                saveSVG();
                                break;
                            case 'btn-outline':
                                outlinenodes = !e.item.checked;
                                if (svg) updateSVG();
                                break;
                            case 'btn-zoomin':
                                zoomByFactor(1.2);
                                break;
                            case 'btn-zoomout':
                                zoomByFactor(0.8);
                                break;
                            case 'btn-legend':
                                insertLegend();
                                break;
                            case 'btn-haplotype':
                                insertHaplotype();
                                break;
                            case 'btn-distance':
                                insertDistance()
                                break;
                            default:
                                /*
                                 * If event.target is none of the above, it must
                                 * be 'btn-lwidth'. Check if true and apply the
                                 * selection
                                 */
                                if (target.indexOf('btn-lwidth:') !== -1) {
                                    linewidth = e.subItem.lwidth;

                                    if (svg) updateSVG();
                                }
                                if (target.indexOf('btn-time:') !== -1) {
                                    typeid = parseInt(e.subItem.typeid);
                                    if (svg) updateSVG();
                                }
                                if (target.indexOf('btn-style:') !== -1) {
                                    styleid = parseInt(e.subItem.styleid);
                                    if (svg) updateSVG();
                                }
                        }
                    },
                },
            }
                //{ type: 'bottom', size: 30, resizable: false, style: style, content: 'Start'}
            ],
        });
        if (w2ui.Layout) return w2ui.Layout; else return null;
    }

    /*
     *
     * legend.js was modified from https://github.com/emeeks/d3-svg-legend/blob/master/legend.js
     * authored by Michael P Schroeder (mpschr)
     *
     * It is used under the following licence (https://github.com/emeeks/d3-svg-legend/blob/master/LICENSE)
     *
     * This is free and unencumbered software released into the public domain.
     *
     * Anyone is free to copy, modify, publish, use, compile, sell, or
     * distribute this software, either in source code form or as a compiled
     * binary, for any purpose, commercial or non-commercial, and by any
     * means.
     *
     * In jurisdictions that recognize copyright laws, the author or authors
     * of this software dedicate any and all copyright interest in the
     * software to the public domain. We make this dedication for the benefit
     * of the public at large and to the detriment of our heirs and
     * successors. We intend this dedication to be an overt act of
     * relinquishment in perpetuity of all present and future rights to this
     * software under copyright law.
     *
     * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
     * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
     * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
     * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
     * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
     * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
     * OTHER DEALINGS IN THE SOFTWARE.
     *
     * For more information, please refer to <http://unlicense.org>
     */


    d3.svg.legend = function () {

        var legendValues = [];
        var cellWidth = 80;
        var cellHeight = 30;
        var labelFormat = d3.format(".01f");
        var coordinates = {x: 0, y: 0};
        var labelUnits = "units";
        var changeValue = 1;
        var orientation = "horizontal";
        var cellPadding = 0;


        function legend(svg) {

            var updateBGSize = function (legend) {

                var margin = 10;
                var dim = legend.target.node().getBBox();
                dim.height += margin * 2;
                dim.width += margin * 2;
                dim.y -= margin;
                dim.x -= margin;

                legend.parentGroup.select(".mutLegendBG").attr(dim);
            };

            var drag = d3.behavior.drag()
                .on("drag", function (d) {
                    d.x += d3.event.dx;
                    d.y += d3.event.dy;
                    d3.select(this).attr("transform", function (d) {
                        return "translate(" + [d.x, d.y] + ")";
                    });
                })
                .on("dragstart", function () {
                    d3.event.sourceEvent.stopPropagation(); // silence other listeners
                });

            function init() {
                var mutLegendGroup = svg.append("g")
                    .attr("class", "mutLegendGroup")
                    .data([coordinates])
                    .attr("transform", "translate(" + coordinates.x + "," + coordinates.y + ")");
                var target = mutLegendGroup
                    .insert("g")
                    .attr("class", "mutLegendGroupText");


                // set legend background
                var mutLegendBG = mutLegendGroup
                    .insert("rect", ":first-child")
                    .attr("class", "mutLegendBG")
                    .attr("fill", "white")
                    .attr("stroke", "black")
                    .attr("stroke-width", "1px");


                return {
                    parentGroup: mutLegendGroup, target: target
                };
            }


            function cellRange(valuePosition, changeVal) {
                legendValues[valuePosition].stop[0] += changeVal;
                legendValues[valuePosition - 1].stop[1] += changeVal;
                redraw();
            }

            function redraw() {
                legend.target.selectAll("g.legendCells").data(legendValues).exit().remove();
                legend.target.selectAll("g.legendCells").select("rect")
                    .style("fill", function (d) {
                        return d.color;
                    });
                if (orientation === "vertical") {
                    legend.target.selectAll("g.legendCells").select("text.breakLabels").style("display", "block").style("text-anchor", "start").attr("x", cellWidth + cellPadding).attr("y", 5 + (cellHeight / 2)).text(function (d) {
                        return labelFormat(d.stop[0]) + (d.stop[1].length > 0 ? " - " + labelFormat(d.stop[1]) : "");
                    });
                    legend.target.selectAll("g.legendCells").attr("transform", function (d, i) {
                        return "translate(0," + (i * (cellHeight + cellPadding)) + ")";
                    });
                } else {
                    legend.target.selectAll("g.legendCells").attr("transform", function (d, i) {
                        return "translate(" + (i * cellWidth) + ",0)";
                    });
                    legend.target.selectAll("text.breakLabels").style("text-anchor", "middle").attr("x", 0).attr("y", -7).style("display", function (d, i) {
                        return i === 0 ? "none" : "block";
                    }).text(function (d) {
                        return labelFormat(d.stop[0]);
                    });
                }
            }

            // init
            if (!legend.initDone) {
                var initObj = init();
                legend.target = initObj.target;
                legend.parentGroup = initObj.parentGroup;
                legend.parentGroup.call(drag);
                legend.initDone = true;
            }


            // remove previously painted rect and text
            legend.target.selectAll("g.legendCells").select("text.breakLabels").remove();
            legend.target.selectAll("g.legendCells").select("rect").remove();
            legend.target.selectAll(".legendTitle").remove();


            legend.target.selectAll("g.legendCells")
                .data(legendValues)
                .enter()
                .append("g")
                .attr("class", "legendCells")
                .attr("transform", function (d, i) {
                    return "translate(" + (i * (cellWidth + cellPadding)) + ",0)";
                });

            legend.target.selectAll("g.legendCells")
                .append("rect")
                .attr("class", "breakRect")
                .attr("height", cellHeight)
                .attr("width", cellWidth)
                .style("fill", function (d) {
                    return d.color;
                })
                .style("stroke", function () {
                    return "#000000"; /*d3.rgb(d.color).darker(); */
                });

            legend.target.selectAll("g.legendCells")
                .append("text")
                .attr("class", "breakLabels")
                .style("pointer-events", "none");

            legend.target.append("text")
                .text(labelUnits)
                .attr("y", -7)
                .attr("class", "legendTitle");

            redraw();
            updateBGSize(legend);
        }

        legend.initDone = false;
        legend.target;

        legend.inputScale = function (newScale) {
            let scale;
            if (!arguments.length) return scale;
            scale = newScale;
            legendValues = [];
            if (scale.invertExtent) {
                //Is a quantile scale
                scale.range().forEach(function (el) {
                    var cellObject = {color: el, stop: scale.invertExtent(el)};
                    legendValues.push(cellObject);
                });
            } else {
                scale.domain().forEach(function (el) {
                    var cellObject = {color: scale(el), stop: [el, ""]};
                    legendValues.push(cellObject);
                });
            }
            return this;
        };

        legend.scale = function (testValue) {
            var foundColor = legendValues[legendValues.length - 1].color;
            for (var el in legendValues) {
                if (testValue < legendValues[el].stop[1]) {
                    foundColor = legendValues[el].color;
                    break;
                }
            }
            return foundColor;
        };

        legend.cellWidth = function (newCellSize) {
            if (!arguments.length) return cellWidth;
            cellWidth = newCellSize;
            return this;
        };

        legend.cellHeight = function (newCellSize) {
            if (!arguments.length) return cellHeight;
            cellHeight = newCellSize;
            return this;
        };

        legend.cellPadding = function (newCellPadding) {
            if (!arguments.length) return cellPadding;
            cellPadding = newCellPadding;
            return this;
        };

        legend.cellExtent = function (incColor, newExtent) {
            var selectedStop = legendValues.filter(function (el) {
                return el.color === incColor;
            })[0].stop;
            if (arguments.length === 1) return selectedStop;
            legendValues.filter(function (el) {
                return el.color === incColor;
            })[0].stop = newExtent;
            return this;
        };

        legend.cellStepping = function (incStep) {
            if (!arguments.length) return changeValue;
            changeValue = incStep;
            return this;
        };

        legend.units = function (incUnits) {
            if (!arguments.length) return labelUnits;
            labelUnits = incUnits;
            return this;
        };

        legend.orientation = function (incOrient) {
            if (!arguments.length) return orientation;
            orientation = incOrient;
            return this;
        };

        legend.labelFormat = function (incFormat) {
            if (!arguments.length) return labelFormat;
            labelFormat = incFormat;
            if (incFormat === "none") {
                labelFormat = function (inc) {
                    return inc;
                };
            }
            return this;
        };

        legend.place = function (incCoordinates) {
            if (!arguments.length) return incCoordinates;
            coordinates = incCoordinates;
            return this;
        };

        return legend;

    };

    /*
    * Reads GML file (output of TCS) from localhost. Should be a .graph file
    * outputed from TCS (format is: Graphic Modelling Language - GML)
    */

    function loadGraph(e) {
        var haplos = [];
        var input = e.target;

        var fileInput = document.getElementById('loadGraph');

        /*
         * Read only one file, no more, no less
         */

        if (input.files.length !== 1) return;

        var reader = new FileReader();


        /*
         * clear nodes and edges lists
         */

        nodeList = [];
        edgeList = [];
        linkList = [];

        reader.onload = function () {
            var text = reader.result;
            var lines = text.split('\n');
            var newnode = false;
            var newedge = false;
            var multilabels = false;
            var frequency, radius, haplogroup, label, changes, source, target;
            var labels = []
            for (var i = 0; i < lines.length; i++) {
                if (lines[i].indexOf('node [') === 3) newnode = true;
                if (lines[i].indexOf('edge [') === 3) newedge = true;
                if (lines[i].indexOf(']') === 3) {
                    if (newnode) {
                        newnode = false;
                        if (labels.length > 0) {
                            radius = 2;
                            if (label !== '') {

                                /* This is a true haplotype (not a transition nodes). Add it
                                 * to the haplotypes' list that will be presented in the grid
                                 * 'haplotypes'. Make sure labels start with a charcater ( not
                                 * a digit) and have only [a-ZA-Z0-9_-] characters. Replace all
                                 * other characters by '_'. The reason for this is that although
                                 * JQuery can handle most of these and other characters as
                                 * element's ids, d3.js is much more restrictive...
                                 */
                                var countsum = 0;


                                for (var j = 0; j < labels.length; j++) {
                                    labels[j] = labels[j].replace(/[^a-zA-Z0-9_=-]/g, "_");
                                    var count;
                                    var timevalue;
                                    var nodestyle = 1;

                                    if (labels[j].includes('=')) {
                                        var parts = labels[j].split('=');
                                        labels[j] = parts[0];
                                        count = parseInt(parts[1]);
                                        timevalue = parseFloat(parts[2]); // Convert to a floating-point number
                                    } else {
                                        count = 1;
                                        nodestyle = 0;
                                    }

                                    countsum += count;

                                    // Update minTime and maxTime values
                                    minTime = Math.min(minTime, timevalue);
                                    maxTime = Math.max(maxTime, timevalue);

                                    /*
                                     * Test if the first characters is a digit. If so, prepend
                                     * the label with 'L'.
                                     */
                                    if (/^\d+$/.test(labels[j][0])) labels[j] = 'L' + labels[j];

                                    haplos.push({
                                        recid: labels[j],
                                        haplogroup: haplogroup,
                                        group: 'Default',
                                        color: 'ffffff',
                                        count: count,
                                        time: timevalue,
                                        nodestyle: nodestyle,
                                    });
                                }

                                // Define a grayscale color scale

                                frequency = countsum;
                                /* Set the SVG radius of the haplogroup to a standard size, based
                                 * on the number of haplotypes in the haplogroup ('frequency')
                                 * times 'standardRadius'. standardRadius is defined as a global variable.
                                 * For transition nodes use a radius = frequency*standardRadius;
                                 */

                                // radius = Math.sqrt(frequency * area / Math.PI);
                                radius = Math.pow(frequency, 1 / 3) * standardRadius;
                            }
                            var labelname = labels.join("\n");
                            /*
                             * Push the nodes (haplogroups) into a nodeList, naming them by
                             * the first label if they include more than one
                             */

                            if (nodestyle === 0) radius = 1;
                            nodeList.push({
                                name: labelname, radius: radius, nodestyle: nodestyle, proportions: [{
                                    group: 'Default',
                                    value: frequency,
                                    radius: radius,
                                    color: '#ffffff',
                                    pattern: 'none'
                                }], id: haplogroup, //  label holds the info which should be shown
                                label: []
                            });

                            /*
                             * clear the labels list for next haplogroup
                             */

                            labels = [];
                        }
                    } else {
                        if (newedge) {
                            newedge = false;
                            edgeList.push({source: source, target: target, id: labels[0], changes: changes});
                        } else {
                            console.log('Serious Error!');
                        }
                    }
                }
                if (newnode) {
                    if ((lines[i][0] === '"') && (multilabels)) multilabels = false;
                    if (multilabels) labels.push(lines[i].trim());
                    if (lines[i].indexOf('id') === 6) haplogroup = Number(lines[i].substr(9, 10).trim());
                    if (lines[i].indexOf('Frequency') === 9) {
                        frequency = Number(lines[i].substr(30, 10).trim());
                        if (frequency > 0) multilabels = true;
                    }
                    if (lines[i].indexOf('label') === 6) label = lines[i].substr(12, 100).replace(/"/g, ' ').trim();
                    //if(lines[i].indexOf('x') == 12) x = Number(lines[i].substr(14, 100).trim());
                    //if(lines[i].indexOf('y') == 12) y = Number(lines[i].substr(14, 100).trim());
                    //if(lines[i].indexOf('group') == 9) group = Number(lines[i].substr(15, 100).trim());
                }
                if (newedge) {
                    if (lines[i].indexOf('label') === 6) label = lines[i].substr(12, 100).replace(/"/g, '').trim();
                    if (lines[i].indexOf('Changes') === 9) changes = lines[i].substr(17, 100).replace(/"/g, '').replace(/\t/g, ' ').trim();
                    if (lines[i].indexOf('source') === 6) source = Number(lines[i].substr(13, 100).trim());
                    if (lines[i].indexOf('target') === 6) target = Number(lines[i].substr(13, 100).trim());
                }
            }
            var colorScale = d3.scale.linear()
                .domain([minTime, maxTime])
                .range([8, 247]);
            if (minTime === maxTime) styleid = 1;
            haplos.forEach(function (haplo) {
                if (haplo.nodestyle) {
                    var grayValue = Math.round(colorScale(maxTime + minTime - haplo.time));
                    haplo.timecolor = grayValue.toString(16).padStart(2, '0');
                } else {
                    haplo.timecolor = '#000000';
                }
            });

            /*
             * Clear any haplotypes present in the haplotypes' grid
             */

            if (w2ui.haplotypes.records.length > 0) w2ui.haplotypes.clear();

            /*
             * Clear any groups defined except Default
             */

            if (w2ui.groups.records.length > 1) {
                w2ui.groups.clear();
                w2ui.groups.add({recid: 'Default', color: 'ffffff', pattern: 'none', editable: false});
            }

            /*
             * Remove any previous svg elements
             */

            if (svg) {
                d3.select("#gview").selectAll("*").remove();
                svg = null;
                force = null;
            }


            /*
             * Add the new haplotypes to the grid
             */

            w2ui.haplotypes.add(haplos);

            /*
             * Disable start button
             */

            $('#start').prop('disabled', true);

            /*
             * clear input field so that file can be reopened!
             */

            fileInput.value = "";

            /*
             * start the d3 force layout
             */

            svgStart();

        };
        reader.readAsText(input.files[0]);
    }

    /*
     * Reads a CSV file with group names and colors
     */

    function loadGroups(event) {

        var input = event.target;
        // Read only one file, no more, no less

        if (input.files.length !== 1) return;

        var fileInput = document.getElementById('loadGroups');

        var reader = new FileReader();

        reader.onload = function () {
            var i, j, k, p;
            var text = reader.result;
            var lines = text.split('\n');
            var line, l, name, color, pattern;
            var names = [];
            var g = [{recid: 'Default', color: 'ffffff', pattern: 'none', editable: false}];
            for (i = 0; i < lines.length; i++) {
                line = lines[i].trim();
                if (line !== '') {
                    l = line.split(";");
                    if (l.length === 2 || l.length === 3) {   // Accept only lines with two or three fields
                        name = l[0].trim();
                        if (name !== '') {                       // There is at least a label or name or something as a first field...
                            k = names.indexOf(name);                // Check if this name is already in list
                            if (k === -1) {
                                names.push(name);                     // Add name to the names' list
                                color = l[1].trim();                    // read the color
                                if (/^#[0-9a-f]{3,6}$/i.test(color)) {  // check if it is a valid RGB color (e.g, #a2ff4b or #a0f)
                                    color = color.substr(1);              // strip the # prefix
                                }
                                pattern = 'none';
                                if (typeof l[2] !== 'undefined') {   // read an optional pattern
                                    p = l[2].trim();
                                    j = pattern_names.filter(function (v) {
                                        return v.id === p;
                                    })[0];
                                    if (typeof j !== 'undefined') pattern = p;
                                }
                                g.push({recid: name, color: color, pattern: pattern});
                            }
                        }
                    }
                }
            }
            if (g.length > 1) {

                /*
                 * Some groups were added, besides the default one, so update w2ui.groups
                 */

                w2ui.groups.clear();
                w2ui.groups.add(g);

                /*
                 * Now check if haplotype list is already defined
                 * and clean any classification made
                 */

                if (w2ui.haplotypes.records.length > 0) {
                    var g;
                    for (i = 0; i < w2ui.haplotypes.records.length; i++) {
                        g = w2ui.haplotypes.records[i].group;
                        w2ui.haplotypes.records[i].group = 'Default';
                        w2ui.haplotypes.records[i].color = 'ffffff';
                        classify(i, 'Default', g);
                    }
                    w2ui.haplotypes.refresh();
                }

                /*
                 * If SVG is already set, reset fill patterns
                 */

                if (svg) {

                    /*
                     * Remove any pattern definition from the SVG
                     */

                    $('pattern').remove();
                    for (i = 0; i < w2ui.groups.records.length; i++) {
                        if (w2ui.groups.records[i].pattern !== 'none') {
                            createPattern(w2ui.groups.records[i].pattern, w2ui.groups.records[i].color);
                        }
                    }
                }

            } else {
                w2alert('This seems not to be a formatted "Group" file!<br>' + '(CSV text file with group names and colors   <br>' + 'separated by a semicolon. Please hit the Help button.', 'No groups loaded!');
            }

            /*
             * clear input field so that file can be reopened!
             */

            fileInput.value = "";
        };

        if (w2ui.groups.records.length > 1) {

            /*
             * Groups already loaded! Override?
             */

            w2confirm('Old list will be deleted! If some haplotypes have<br>' + 'already been associated with groups (colors) they<br>' + 'may revert to default group or become associated <br>' + 'with a different group                           <p>' + 'Load the new list?', 'Replace active group\'s list?')
                .yes(function () {
                    reader.readAsText(input.files[0]);
                });
        } else {
            reader.readAsText(input.files[0]);
        }
    }


    // todo: read a csv file with the characters
    function loadTraits(e) {

        var input = e.target;

        /*
         * Read only one file, no more, no less
         */

        if (input.files.length !== 1) return;

        var fileInput = document.getElementById('loadTraits');

        var reader = new FileReader();

        reader.onload = function () {
            var text = reader.result.toUpperCase();
            var lines = text.split('\n');
            var headers = lines[0].split(',').map(function (header) {
                return header.trim();
            });

            // 存储所有样本的所有trait信息
            var sampleTraitsInfo = {};

            // Iterate through each column starting from the second column
            for (let i = 1; i < headers.length; i++) {
                let traitDict = {};

                // Iterate through each row starting from the second line
                for (let j = 1; j < lines.length; j++) {
                    let line = lines[j].trim();
                    if (line === '') continue; // Skip empty lines

                    let row = line.split(',');
                    let name = row[0].trim(); // Clean the name once

                    // Ensure that the row has enough columns
                    if (row.length > i) {
                        traitDict[name] = row[i].trim(); // Assign the value to the characters dictionary
                    }
                }
                sampleTraitsInfo[headers[i]] = traitDict;
            }

            var traitsInfo = [];
            Object.keys(sampleTraitsInfo).forEach(function (traitName) {
                var traitDetail = Array.from(new Set(Object.values(sampleTraitsInfo[traitName])));
                traitsInfo.push({recid: traitName, traitNum: traitDetail.length, traitDetail: traitDetail});
            });


            w2ui.characters.clear();
            w2ui.characters.records = traitsInfo;
            w2ui.characters.sampleTraitsInfo = sampleTraitsInfo;
            w2ui.characters.refresh();
        }

        // todo: 完善该函数 允许重新打开一个文件并实现

        /*
             * clear input field so that file can be reopened!
             */

        fileInput.value = "";

        reader.readAsText(input.files[0]);
    }


    /*
     * Input a list of traits to generate the hapconf file
     */
    function generateHapconffile(sampleTraitInfo) {
        /*
            traitInfo is a dict
            e. {DP0005: "DP", DP0027: "DP", DP0037: "DP", DP0058: "DP", DP0059: "DP"}
         */

        var haploInfo = Object.entries(sampleTraitInfo).map(([key, value]) => `${key};${value}`);
        // 创建目标变量对象
        let target = {files: []};

        // 将键值对格式化并添加到目标变量的文件数组中
        target.files.push(new File([haploInfo.join('\n') + '\n'], ".gml"));
        return {target};
        // return [haploInfo.join('\n')+'\n'];
    }


    /*
     * Input a trait to generate the groupconf file
     */
    function generateGroupconffile(traitRecord) {
        /*
         * set a default
         */
        const color = {
            3: ["#D85356", "#D7AA36", "#94BBAD"],
            4: ["#F0776D", "#FBD2CB", "#29B4B6", "#C5E7E8"],
            5: ["#8386A8", "#D15C6B", "#F5CF36", "#8FB943", "#78B9D2"],
            6: ["#ce3d8a", "#FF8748", "#5BAA56", "#B8BB5B", "#4186B7", "#8679BE"],
            7: ["#794292", "#44599B", "#2C8FA0", "#40A93B", "#EFE644", "#E97124", "#ec77c7"],
            8: ["#FF7F00", "#FDBF6F", "#dc70d9", "#FB9A99", "#33A02C", "#B2DF8A", "#1F78B4", "#A6CEE3"],
            9: ["#ce468a", "#2CB8BB", "#55B333", "#bf5ee1", "#1a5031", "#A4C5E8", "#EF7A1C", "#F9CA9C", "#3b3a3a"],
            10: ["#009F72", "#D25F27", "#E49E21", "#EFE341", "#1F78B4", "#FB9A99", "#bd60d7", "#EAD59F", "#C9B1D4", "#52310F"]
        };

        var traitNum = traitRecord["traitNum"];
        var traitDetail = traitRecord["traitDetail"];

        // 根据traitInfo生成groupInfo
        let groupInfo = [];
        if (traitNum >= 3 && traitNum <= 10) {
            const keys = Object.keys(color[traitNum]);
            traitDetail.forEach((trait, index) => {
                const keyIndex = index % keys.length;
                groupInfo.push(`${trait};${color[traitNum][keyIndex]};none`);
            });
        } else {
            traitDetail.forEach(trait => {
                const randomColor = "#" + Math.floor(Math.random() * 16777215).toString(16); // Generate random color
                groupInfo.push(`${trait};${randomColor};none`);
            });
        }

        // 创建目标变量对象
        let target = {files: []};

        // 将groupInfo添加到目标变量的文件数组中
        target.files.push(new File([groupInfo.join('\n') + '\n'], ".gml"));
        return {target};
        // return [groupInfo.join('\n')+'\n'];
    }


    /*
    * Reads a CSV file with haplotype names and groups
    */

    function loadHaplotypes(e) {

        var input = e.target;

        /*
         * Read only one file, no more, no less
         */

        if (input.files.length !== 1) return;

        var fileInput = document.getElementById('loadHaplotypes');

        var reader = new FileReader();

        reader.onload = function () {

            var text = reader.result;
            var lines = text.split('\n');
            var line, l, name, group, seq2hap;
            var i, h = [];


            for (i = 0; i < lines.length; i++) {
                line = lines[i].trim();
                if (line !== '') {

                    l = line.split(";");

                    /*
                     * Accept only lines with three fields
                     */

                    if (l.length === 3) {
                        name = l[0].trim();
                        if (name !== '') {

                            /*
                             * There is at least a label or name or something as a first field...
                             * Standardize the string (only alpha/numbers and _ and - allowed
                             * If name starts as anumber, preppend an 'L' as loadGraph does
                             */

                            name = name.replace(/[^a-zA-Z0-9_=-]/g, "_");

                            if (/^\d+$/.test(name[0])) name = 'L' + name;

                            /*
                             * Now read the second column (the group)
                             */

                            group = l[1].trim();
                            seq2hap = l[2].trim();
                            if (group !== "") {
                                h.push({label: name, group: group, seq2hap: seq2hap})
                            } else {
                                h.push({label: name, group: 'Default', seq2hap: seq2hap});
                            }

                        }
                    }
                }
            }

            /*
             * Check if a list of haplotypes <-> groups was created
             */

            if (h.length > 0) {


                /*
                 * Iterate through all haplotypes in the list, check if they exist
                 * in the haplotype grid and change their group accordingly. Check
                 * also if the group is defined in the group's list. Otherwise,
                 * don't change anything (keep the default group)
                 */

                var hap, grp, ogrp;
                for (i = 0; i < h.length; i++) {

                    /*
                     * Try to find this label in haplotypes' grid and
                     * return its index. Accept only the first value
                     * retreived (should be a single value as labels
                     * are unique keys, but users may wrongly provide
                     * incomplete haplotype labels...
                     */

                    hap = w2ui.haplotypes.find({recid: h[i].label}, true)[0];

                    /*
                     * Try to find this group in groups' grid and
                     * return its index. Since the group's list doesn't
                     * allow for duplicate keys, the function returns
                     * either a record number (index) or -1 (not found)
                     */

                    grp = w2ui.groups.find({recid: h[i].group}, true)[0];

                    if ((typeof hap !== 'undefined') && (typeof grp !== 'undefined')) {

                        /*
                         * Retreive the value of the 'group' for this record
                         */

                        ogrp = w2ui.haplotypes.records[hap].group;
                        if (typeof ogrp === 'undefined') ogrp = 'Default';

                        /*
                         * Change groups
                         */

                        w2ui.haplotypes.records[hap].group = h[i].group;
                        w2ui.haplotypes.records[hap].color = w2ui.groups.records[grp].color;
                        w2ui.haplotypes.records[hap].seq2hap = h[i].seq2hap

                        /*
                         * Reclassify this haplotype
                         */

                        classify(hap, h[i].group, ogrp);
                    }
                }
                updateSVG();
                w2ui.haplotypes.refresh();
            } else {
                w2alert('This seems not to be a formatted "Haplotype" file!<br>' + '(CSV text file with haplotype and group names     <br>' + 'separated by a semicolon. Please hit the Help button.', 'No haplotypes loaded!');
            }

            /*
             * clear input field so that file can be reopened!
             */

            fileInput.value = "";
        };

        if (w2ui.groups.records.length > 1) {  //Groups already loaded! Override?
            w2confirm('Colors will be replaced in the haplotype list! If<br>' + 'any color has already been assigned they will be<br>' + 'overriden! Load the colors\' list, anyway?       <br>', 'Replace active group\'s list?').yes(function () {
                reader.readAsText(input.files[0]);
            });
        } else {
            reader.readAsText(input.files[0]);
        }
    }


    function saveExamples(fileName) {
        if (filesave) {

            // for non-IE
            if (!window.ActiveXObject) {
                var save = document.createElement('a');
                save.href = 'examples/' + fileName;
                save.target = '_blank';
                save.download = fileName || 'unknown';

                var event = document.createEvent('Event');
                event.initEvent('click', true, true);
                save.dispatchEvent(event);
                (window.URL || window.webkitURL).revokeObjectURL(save.href);
            }

            // for IE
            else if (!!window.ActiveXObject && document.execCommand) {
                var _window = window.open('examples/' + fileName, '_blank');
                _window.document.close();
                _window.document.execCommand('SaveAs', true, fileName || 'examples/' + fileName);
                _window.close();
            }


            /*
             * Group list exists
             */
            /*
            if( w2ui.groups.records.length > 0 ){
              for( var i = 1; i < w2ui.groups.records.length; i++ ) {
                label   = w2ui.groups.records[i].recid;
                color   = w2ui.groups.records[i].color;
                pattern = w2ui.groups.records[i].pattern;
                list.push(label+';#'+color+';'+pattern+'\n');
              }
              var blob = new Blob(list, {type: "text/plain;charset=utf-8"}, {endings: "native"});
              saveAs(blob, "groups.csv");
            }
            */
        } else {
            w2alert('FileSaver.js is not supported! Use a modern browser...<br>' + 'FileSaver.js is supported by Firefox 20+, Chrome, Chrome<br>' + 'for Android, IE 10+, Opera 15+ and Safari 6.1+', 'FileSave.js is unsupported!');
        }
    }

    function saveGroups() {
        if (filesave) {
            var label, color, pattern;
            var list = [];

            /*
             * Group list exists
             */

            if (w2ui.groups.records.length > 0) {
                for (var i = 1; i < w2ui.groups.records.length; i++) {
                    label = w2ui.groups.records[i].recid;
                    color = w2ui.groups.records[i].color;
                    pattern = w2ui.groups.records[i].pattern;
                    list.push(label + ';#' + color + ';' + pattern + '\n');
                }
                var blob = new Blob(list, {type: "text/plain;charset=utf-8"}, {endings: "native"});
                saveAs(blob, "groups.csv");
            }
        } else {
            w2alert('FileSaver.js is not supported! Use a modern browser...<br>' + 'FileSaver.js is supported by Firefox 20+, Chrome, Chrome<br>' + 'for Android, IE 10+, Opera 15+ and Safari 6.1+', 'FileSave.js is unsupported!');
        }
    }


    function highlightHaplotypes() {
        /*
         * a function to highlight the selected record (which means the nodes in the graph)
         */
        // 获取选择的Recoed的id
        var selectedRecordIds = w2ui.haplotypes.getSelection();
        if (selectedRecordIds.length === 0) {
            w2alert("Please select at least one record!", "Warning");
        }

        // 实现高亮列的处理 已经高亮的取消高亮 没有高亮的进行高亮
        selectedRecordIds.forEach(function (recid) {
            w2ui.haplotypes.get(recid).highlight = !w2ui.haplotypes.get(recid).highlight;
        });
        w2ui.haplotypes.refresh();


        // 获取haplogroupId
        var highlightedHaplogroupIds = w2ui.haplotypes.records
            .filter(function (record) {
                return record.highlight; // 筛选出 highlight 属性为 true 的记录
            })
            .map(function (record) {
                return record.haplogroup; // 从筛选出的记录中提取 haplogroup
            });

        // 去除重复元素
        var uniqueHaplogroupIds = new Set(highlightedHaplogroupIds);
        // 将Set转换为数组并应用map 在updateSvg中将id转化为了name 所以这里获取到的name是用于ID属性

        // 更新全局变量
        highlightNode = Array.from(uniqueHaplogroupIds).map(function (id) {
            return getNameById(id);
        });
        updateSVG();
    }

    function insertHaplotype() {
        /***
         a function to show the haplotype of the nodes on the svg
         ***/
        seqHapFlag = !seqHapFlag;

        if (!seqHapAdded) {
            const haplogroupMapping = w2ui.haplotypes.records.reduce((mapping, record) => {
                // 使用 haplogroup 作为键，seq2hap 作为值
                mapping[record.haplogroup] = record.seq2hap;
                return mapping;
            }, {});
            nodeList.forEach(node => {
                node.seq2hap = haplogroupMapping[node.id] || '';
            });
            seqHapAdded = !seqHapAdded;
        }
        updateSVG();
    }

    function insertDistance() {
        /***
         a function to show the distance between each two nodes
         ***/
        distanceFlag = !distanceFlag;
        updateSVG();
    }

    function labelHaplotype() {
        /***
         a function to add texts on the svg based on the selected records
         ***/

            // 获取选择的Recoed的id
        var selectedRecordIds = w2ui.haplotypes.getSelection();
        if (selectedRecordIds.length === 0) {
            w2alert("Please select at least one record!", "Warning");
        }

        // 实现标签列的处理 已经添加标签的取消添加 没有添加标签的添加标签
        selectedRecordIds.forEach(function (recid) {
            w2ui.haplotypes.get(recid).label = !w2ui.haplotypes.get(recid).label;
        });
        w2ui.haplotypes.refresh();


        var labelinfo = {};
        // 筛选label为true的record 获取其haplogroup 并定位到svg中的node
        w2ui.haplotypes.records.filter(record => record.label).forEach(record => {
            var haplogroup = record.haplogroup;
            // 使用 getNameById 函数转换 haplogroup ID
            var name = getNameById(haplogroup);
            // 将 recid 添加到对应 haplogroup 的数组中，如果该数组不存在则创建一个新数组
            labelinfo[name] = labelinfo[name] || [];
            labelinfo[name].push(record.recid);
        });

        // 更新全局变量
        labelNode = labelinfo;
        updateSVG();
    }


    function getNameById(nodeId) {
        var node = nodeList.find(node => node.id === nodeId);
        return node ? node.name : null;
    }

    function saveHaplotypes() {
        if (filesave) {
            var label, group, seq2hap;
            var list = [];

            /*
             * Haplotype list has been loaded
             */

            if (w2ui.haplotypes.records.length > 0) {
                for (var i = 0; i < w2ui.haplotypes.records.length; i++) {
                    label = w2ui.haplotypes.records[i].recid;
                    group = w2ui.haplotypes.records[i].group;
                    seq2hap = w2ui.haplotypes.records[i].seq2hap;
                    list.push(label + ';' + group + ';' + seq2hap + '\n');
                }
                var blob = new Blob(list, {type: "text/plain;charset=utf-8"}, {endings: "native"});
                saveAs(blob, "haplotypes.csv");
            }
        } else {
            w2alert('FileSaver.js is not supported! Use a modern browser...<br>' + 'FileSaver.js is supported by Firefox 20+, Chrome, Chrome<br>' + 'for Android, IE 10+, Opera 15+ and Safari 6.1+', 'FileSave.js is unsupported!');
        }
    }

    function saveSVG() {
        if (filesave) {
            var d = $('#gview');
            var blob = new Blob([d.html()], {type: "text/plain;charset=utf-8"});
            saveAs(blob, "network.svg");
        }
    }

    function svgSave() {
        try {
            var isFileSaverSupported = !!new Blob;
            var d = $('#gview');
            var blob = new Blob([d.html()], {type: "text/plain;charset=utf-8"});
            saveAs(blob, "network.svg");
        } catch (e) {
            w2alert('FileSaver.js is not supported! Use a modern browser...<br>' + 'FileSaver.js is supported by Firefox 20+, Chrome, Chrome<br>' + 'for Android, IE 10+, Opera 15+ and Safari 6.1+', 'File Save Failed!');
        }
    }

    function zoomByFactor(factor) {
        if (!zoom) return;
        var scale = zoom.scale();
        var extent = zoom.scaleExtent();
        var newScale = scale * factor;
        var w = $('#gview').width();
        var h = $('#gview').height();
        if (extent[0] <= newScale && newScale <= extent[1]) {
            var t = zoom.translate();
            var c = [w / 2, h / 2];
            zoom
                .scale(newScale)
                .translate([c[0] + (t[0] - c[0]) / scale * newScale, c[1] + (t[1] - c[1]) / scale * newScale])
                .event(svg.transition().duration(350));
        }
    }

    function svgStart() {

        var massFactor = 0;
        var lnkdist = defaultLinkDistance;
        var lnkstre = defaultLinkStrength;
        var frict = defaultFriction;
        var chrg = defaultCharge;
        // Do not mess with this
        // var chrgdist = Infinity;
        var grav = defaultGravity;

        /*
         * clear linkList if not empty
         */

        linkList = [];

        /*
         * arry of nodes is in global var 'nodeList'
         * array of edges is in global variable 'edgeList'
         *
         * Sort nodes based on radius size (frequency) so that smaller nodes
         * are added last to the list and will be on top of the bigger
         * ones, thus avoiding being hidden by the latter upon draw
         * */

        nodeList.sort(function comp(a, b) {
            var r = (a.radius < b.radius) ? 1 : (a.radius > b.radius) ? -1 : 0;
            return r;
        });

        /* The 'edges' array has source/target node pairs. However, since nodeList was
         * sorted, these pairs are not synced with the index of the nodes array anymore.
         * We build a 'linkList' array by pushing from the nodeList array the 'target'
         * and 'source' nodes and passing them as objects for the linkList. We also
         * use the radius of the nodes to set an ideal link distance which is equal to the
         * sum of their radii, plus the default distance between two nodes with radii = 1
         */

        var pair;

        for (var i = 0; i < edgeList.length; i++) {
            pair = nodeList.filter(function (e) {
                return (e.id === edgeList[i].source) || (e.id === edgeList[i].target);
            });
            if (pair.length === 2) linkList.push({
                source: pair[0],
                target: pair[1],
                id: "Link_" + pair[0].id + "-" + pair[1].id,
                ldist: pair[0].radius + pair[1].radius + lnkdist + defaultDistance,
                changes: edgeList[i].changes
            });
        }

        /*
         * Redraw elements on zoom/pan
         */

        zoom = d3.behavior.zoom().on("zoom", zoom_redraw);

        function zoom_redraw() {
            svg.attr("transform", "translate(" + zoom.translate() + ")" + "scale(" + zoom.scale() + ")");
        }

        /*
         * Compute 'view' width and height
         */

        var w = $('#gview').width();
        var h = $('#gview').height();

        /*
         * Remove paper advertisement from main DIV
         */

        $('#gview').empty();

        /*
         * Define the main svg element
         */

        svg = d3.select('#gview')
            .append('svg')
            .attr('id', 'SVG')
            .attr("width", "100%")
            .attr("height", "100%")
            .attr("viewBox", "0 0 " + w + " " + h)
            .attr("preserveAspectRatio", "xMidYMid meet")
            //.call(d3.behavior.zoom().on("zoom", zoom_redraw))
            .call(zoom)
            .append('g');

        /*
         * Define a baseline force layout
         */

        force = d3.layout.force()
            .charge(function (d) {
                if (d) return d.radius * chrg; else return chrg;
            })
            .linkDistance(function (d) {
                if (d) return d.ldist * lnkdist; else return lnkdist;
            })
            .gravity(grav)
            .friction(frict)
            .linkStrength(lnkstre)
            .chargeDistance(Infinity)
            .size([w, h])
            .on('end', function () {
                $('#stop').prop('disabled', true);
                $('#start').prop('disabled', false);
            })
            .on('start', function () {
                $('#stop').prop('disabled', false);
                $('#start').prop('disabled', true);
            });

        /*
         * Define a layout for pie charts
         */

        pie = d3.layout.pie()
            .sort(null)
            .value(function (d) {
                return d.value;
            });
        /*
         * Define an arc
         */

        sector = d3.svg.arc()
            .outerRadius(function (d) {
                return d.data.radius;
            })
            .innerRadius(0);

        sectorOuter = d3.svg.arc()
            .outerRadius(function (d) {
                return d.data.radius * 1.4;
            })
            .innerRadius(function (d) {
                return d.data.radius * 0.7;
            });

        sectorSmall = d3.svg.arc()
            .outerRadius(function (d) {
                return d.data.radius * 0.7;
            })
            .innerRadius(0);

        sectorNull = d3.svg.arc()
            .outerRadius(0)
            .innerRadius(0);
        /*
         * Create SVG patterns for the groups defined (except for default group 0)
         */

        defs = svg.append("defs");

        for (var i = 1; i < w2ui.groups.records.length; i++) {
            if (w2ui.groups.records[i].pattern !== 'none') createPattern(w2ui.groups.records[i].pattern, w2ui.groups.records[i].color);
        }

        /*
         * Define what to do ineach iteration of force layout
         */

        force.on("tick", function () {
            link.attr("x1", function (d) {
                return d.source.x;
            })
                .attr("y1", function (d) {
                    return d.source.y;
                })
                .attr("x2", function (d) {
                    return d.target.x;
                })
                .attr("y2", function (d) {
                    return d.target.y;
                });

            if (distanceFlag) {
                linkText
                    .attr('x', function (d) {
                        return (d.source.x + d.target.x) / 2;
                    })
                    .attr('y', function (d) {
                        return (d.source.y + d.target.y) / 2;
                    });
            }

            node.attr("x", function (d) {
                return d.x;
            })
                .attr("y", function (d) {
                    return d.y;
                })
                .attr("transform", function (d) {
                    return "translate(" + d.x + "," + d.y + ")";
                });
        });

        /*
         * Define costum drag
         */

        drag = force.drag().on("dragstart", function (d) {
            d3.event.sourceEvent.stopPropagation();
        });
        force.stop();
        /*
         * Delete a selected node
         */

        clickNode = function (e) {
            if (deletenode) {
                nodeList.splice(nodeList.indexOf(e), 1);
                var toSplice = linkList.filter(function (l) {
                    return (l.source === e) || (l.target === e);
                });
                toSplice.map(function (l) {
                    linkList.splice(linkList.indexOf(l), 1);
                });
                updateSVG();
            }
        };

        /*
         * Delete a selected link
         */

        clickLink = function (e) {
            if (deletelink) {
                linkList.splice(linkList.indexOf(e), 1);
                updateSVG();
            }
        };

        /*
        function massChanged(path, e){
          var arcOver = d3.svg.arc().outerRadius(function(d) { return d.data.radius*e; });
          path.transition().duration(1000).attr("d", arcOver);
         // force.start();
        };
        $('#massFactor').on('change', function (event){ event.onComplete = massChanged(path, event.target.value) } );
        */

        /*
         * charge changed during operation
         */

        function chargeChanged(e) {
            force.stop();
            chrg = e;
            force.start();
        }

        $('#charge').on('change', function (event) {
            event.onComplete = chargeChanged(event.target.value);
        });

        /*
         * linkDistance changed
         */

        function linkDistanceChanged(e) {
            force.stop();
            lnkdist = e;
            force.start();
        }

        $('#linkDistance').on('change', function (event) {
            event.onComplete = linkDistanceChanged(event.target.value);
        });

        /*
         *  linkStrength changed
         */

        function linkStrengthChanged(e) {
            force.stop().linkStrength(e).start();
        }

        $('#linkStrength').on('change', function (event) {
            event.onComplete = linkStrengthChanged(event.target.value);
        });

        /*
         * friction changed
         */

        function frictionChanged(e) {
            force.stop().friction(e).start();
        }

        $('#friction').on('change', function (event) {
            event.onComplete = frictionChanged(event.target.value);
        });

        /*
         * chargeDistance changed
         *
         * Better not to mess with this
         */

        /*
        function chargeDistanceChanged(e){
          //console.log('Dist:',e)
          //if(e==='') e =100000;
          force.stop().chargeDistance(e).start();
          //if(e>10000) e = Infinity;
          //force.start();
        };
        $('#chargeDistance').on('change', function (event){ event.onComplete = chargeDistanceChanged(event.target.value) } );
        */

        /*
         * gravity changed
         */

        function gravityChanged(e) {
            force.stop().gravity(e).start();
        }

        $('#gravity').on('change', function (event) {
            event.onComplete = gravityChanged(event.target.value);
        });

        /*
         * update current/default values in UI controld
         */

        $('#linkDistance').attr('value', lnkdist);
        $('#linkStrength').attr('value', lnkstre);
        $('#friction').attr('value', frict);
        $('#charge').attr('value', chrg);
        // Do not mess with this
        //$('#chargeDistance').attr('value', chrgdist);
        $('#gravity').attr('value', grav);
        $('#lwidth').attr('value', linewidth);
        $('#typeid').attr('value', typeid);

        /*
         * Disable 'start' button
         */

        $('#start').prop('disabled', true).on('click', function (event) {
            /*
             * Disable 'start' button
             */
            $('#start').prop('disabled', true);
            /*
             * Enable stop button
             */
            $('#stop').prop('disabled', false);
            updateSVG();
        });

        $('#stop').prop('disabled', false).on('click', function (event) {
            /*
             * Disable 'stop' button
             */
            $('#stop').prop('disabled', true);
            /*
             * Enable 'start' button
             */
            $('#start').prop('disabled', false);
            force.stop();
        });

        $('#reset').prop('disabled', false);

        /*
         * Enable editing buttons
         */

        w2ui.Layout_main_toolbar.enable('btn-dellink', 'btn-delnode', 'btn-svgsave', 'btn-outline', 'btn-lwidth', 'btn-zoomin', 'btn-zoomout', 'btn-legend', 'btn-time', 'btn-style', 'btn-haplotype', 'btn-distance');


        /*
         * Now star the force layout algorithm and update the SVG!
         */

        updateSVG();

    }

    /*
     * Check for File API support. If enabled, go on...
     */

    if (window.File && window.FileReader && window.FileList && window.Blob) {

        /*
         * Check if FileSave.js works
         */

        try {
            var isFileSaverSupported = !!new Blob();
            filesave = true;
        } catch (e) {
            w2alert('FileSaver.js is not supported! Use a modern browser...<br>' + 'FileSaver.js is supported by Firefox 20+, Chrome, Chrome<br>' + 'for Android, IE 10+, Opera 15+ and Safari 6.1+', 'File Save Failed!');
        }

        /*
         * Define a general style for the GUI (w2ui)
         */

        const style = 'background-color: #F5F6F7; border: 1px solid silver; padding: 3px';

        const groups = getGroupsGrid(style);
        const haplotypes = getHaplotypesGrid(style);

        const characters = getTraitsGrid(style);

        /*
         * Create a new layout
         */

        const layout = getLayout(style, groups, haplotypes, characters);

        layout.html('top', '<div style="float: left;"><button id="loadData" class="w2ui-btn">Load Data</button></div><div style="float: right;"><button id="help" class="w2ui-btn">Help</button></div>');
        layout.html('left', w2ui.haplotypes);
        layout.html('right', '<div style="width=100%; height=100%;">' + '<div style="width=100%; height=100%; align=left">' + '<p></p>' + //'<div class="w2ui-field"><label>Mass:</label><div><input type="text" id="massFactor" /></div></div>'+
            '<div class="w2ui-field"><label>Link Distance:</label><div><input type="text" id="linkDistance" /></div></div>' + '<div class="w2ui-field"><label>Link Strength:</label><div><input type="text" id="linkStrength"  /></div></div>' + '<div class="w2ui-field"><label>Friction:</label><div><input type="text" id="friction"  /></div></div>' + '<div class="w2ui-field"><label>Charge:</label><div><input type="text" id="charge"  /></div></div>' + // Do not mess with this
            //'<div class="w2ui-field"><label>Charge Distance:</label><div><input type="text" id="chargeDistance"  /></div></div>'+
            '<div class="w2ui-field"><label>Gravity:</label><div><input type="text" id="gravity" /></div></div>' + '<p>' + '<div style="text-align: center;"><button class="w2ui-btn" id="start" name="start" disabled>Start</button></div><p>' + '<div style="text-align: center;"><button class="w2ui-btn" id="stop" name="stop" disabled>Stop</button></div>' + '</div>' + //'<div style="margin: 10px; padding-top: 6px; width: 90%; height: 100%; background-color: #EFF0F1;">' +
            //'<p style="text-align: center;">Examples</p>' +
            //'<div style="text-align: center;"><button class="w2ui-btn" id="tcsdata" name="tcsdata">TCS network</button></div><p>'+
            //'<div style="text-align: center;"><a href="examples/tcs-output.graph" target=_balnk>tcs-output.graph</a></div><p>' +
            //'<div style="text-align: center;"><button class="w2ui-btn" id="groups" name="groups">Classification Groups</button></div><p>'+
            //'<div style="text-align: center;"><a href="examples/groups.csv" target=_balnk>groups.csv</a></div><p>' +
            //'<div style="text-align: center;"><button class="w2ui-btn" id="haplotypes" name="haplotypes">Classified Haplotypes</button></div><p>'+
            //'<div style="text-align: center;"><a href="examples/haplotypes.csv" target=_balnk>haplotypes.csv</a></div><p>' +
            //'<div style="text-align: center;"><button class="w2ui-btn" id="gpatterns" name="gpatterns">Groups with patterns</button></div><p>'+
            //'<div style="text-align: center;"><a href="examples/group-patterns.csv" target=_balnk>group-patterns.csv</a></div><p>' +
            //'</div>' +
            '</div>');
        layout.html('main', '<div id="gview" style="width=100%; height=100%;">' + '<div style="width=50%; padding: 20%; font-size: 160%;">' + 'A paper describing tcsBU has been published in the journal <i>Bioinformatics</i>. ' + 'Please cite as: <p>Santos, AM, Cabezas MP, Tavares AI, Xavier R, ' + 'Branco M (2016) tcsBU: a tool to extend TCS network layout and ' + 'visualization. <i>Bioinformatics</i>, btv636 ' + '(<a href="https://academic.oup.com/bioinformatics/article/32/4/627/1744448/" ' + 'target="blank">doi: 10.1093/bioinformatics/btv636</a>)' + '</div>' + '</div>');
        layout.html('bottom', '');

        /*
         * Add a hidden <input> field to load a graph file (GML)
         */

        $('body').append('<input id="loadGraph" type="file" />');


        $('#loadGraph').on('change', function (event) {
            loadGraph(event);
        });
        $('#loadData').click(function () {
            $('#loadGraph').click();
        });

        //$('#massFactor').w2field('float', { min: 1, max: 10, step: 0.5, arrows: true });
        $('#linkDistance').w2field('float', {min: 0.1, max: 10, step: 0.1, arrows: true});
        $('#linkStrength').w2field('float', {min: 0, max: 1, step: 0.02, arrows: true});
        $('#friction').w2field('float', {min: 0, max: 1, step: 0.02, arrows: true});
        $('#charge').w2field('int', {min: -1000, max: 1000, step: 10, arrows: true});
        // Do not mess with this!
        // $('#chargeDistance').w2field('int', { min: 0, step: 10, arrows: true });
        $('#gravity').w2field('float', {min: 0, max: 1, step: 0.001, arrows: true});

        $('#help').click(function () {
            $().w2popup({
                url: 'docs/help.html', title: 'tcsBU HELP', width: 800, height: 500,
            });
        });
    } else {
        w2alert('The File APIs are not fully supported by your browser.');
    }

    // todo: delete
    loadGraph(gmlfile);
    loadGroups(groupconffile);
    loadHaplotypes(hapconffile);

});
