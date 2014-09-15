var cheerio = require("cheerio");

$ = cheerio.load('<ul id="fruits"><p>aaa</p></ul>');

console.log($("ul > p").html());