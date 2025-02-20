import typescript from '@rollup/plugin-typescript';
import {terser} from "rollup-plugin-terser";
import cleanup from "rollup-plugin-cleanup";

export default {
    input: 'src/localization.ts',
    output: [
        {
            file: 'dist/localization.esm.js',
            format: 'esm',
            sourcemap: true,
        },
        {
            file: 'dist/localization.js',
            format: 'umd',
            name: 'window',
            exports: 'named',
            sourcemap: true,
            extend: true,
        },
        {
            file: 'dist/localization.min.js',
            format: 'umd',
            name: 'window',
            exports: 'named',
            sourcemap: true,
            extend: true,
            plugins: [terser({
                    format: {comments: false}
                }
            )]
        },
    ],
    plugins: [
        // remove tslib copyright notice (0BSD doesn't require it)
        cleanup({
            comments: 'none',
        }),
        typescript({
            tsconfig: './tsconfig.json',
            outDir: 'dist',
            declarationDir: 'dist',
        }),
    ],
};