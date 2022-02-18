const path = require("path");
const tsNameof = require("ts-nameof");

module.exports = {
    entry: {
        site: "./Scripts/site.ts"
    },
    output: {
        path: path.resolve(__dirname, "wwwroot/js"),
        filename: "[name].bundle.js"
    },
    resolve: {
        extensions: [".ts", ".js", ".json"]
    },
    module: {
        rules: [
            // all files with a '.ts' extension will be handled by 'ts-loader'
            {
                test: /\.ts$/,
                use: [{
                    loader: "ts-loader",
                    options: {
                        getCustomTransformers: () => ({ before: [tsNameof] })
                    }
                }],
                exclude: /node_modules/
            }
        ]
    },
    // devtool: "cheap-source-map",
    // devtool: "source-map",
    optimization: {
        minimize: false
    }
}
