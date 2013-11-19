all:
	latex TFG_Main.tex
# Genera la bibliografía
	bibtex TFG_Main
# Genera el índice alfabético según el fichero de estilo indiceAlfabetico_sty.ist
#	makeindex TFG_Main.idx	# Generaría el índice alfabético con el estilo "por defecto"
	makeindex -s classic.ist TFG_Main.idx
# Compilar un par de veces o tres para crear las referencias cruzadas de forma adecuadas
	latex TFG_Main.tex
	latex TFG_Main.tex
	latex TFG_Main.tex
	
# Genera el .pdf
	dvipdf TFG_Main

# Genera el .ps
#	dvips -o TFG_Main.ps  TFG_Main.dvi
#	ps2pdf TFG_Main.ps



# cleaning everything that can be automatically recreated with "make".
clean:	
	rm TFG_Main.aux TFG_Main-blx.bib  TFG_Main.bbl TFG_Main.blg TFG_Main.idx TFG_Main.log TFG_Main.out TFG_Main.run.xml TFG_Main.toc
